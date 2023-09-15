using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public abstract class BaseMapBuildingPathGen : MapGenComponent {

    [Header("ID")]
    public int uniqueComponentID = 0;   //Please make sure this is unique

    [Header("Generate")]
    public int generationTryCount = 10;
    public int maxConnectLegth = 10;
    public MapPoint.PointType pointType;
    public LayerMask blockMask;

    private MapPointCollection[] mapPointsCollection;

    public struct PathPoint {
        public Vector3 startPos;
        public Vector3 endPos;
    }

    public override void Initialize() {

        base.Initialize();

        if (GameManagerBase.instance.isServer) {

            Random.InitState(MapGenData.seed);

            //Look for the point
            mapPointsCollection = GameObject.FindObjectsOfType<MapPointCollection>(true).Where(t => t.ContainType(pointType) == true).ToArray();

        }

    }

    public override IEnumerator Process() {

        if (GameManagerBase.instance.isServer) {

            if (mapPointsCollection.Length == 0)
                yield break;

            ArrayList detectedMapPoint = new ArrayList();

            //Loop to try to spawn
            for (int i = 0; i < generationTryCount; i++) {

                //Look for random point
                var index = Random.Range(0, mapPointsCollection.Length);

                var startPoint = mapPointsCollection[index];

                //Look for nearby point
                detectedMapPoint.Clear();   //Make sure we clear first
                int detectedCount = DetectedNearby(startPoint.gameObject, ref detectedMapPoint);

                if (detectedCount <= 0)
                    continue;

                yield return null;

                //Try connect within this range of point
                for (int id = 0; id < detectedCount; id++) {

                    //Find a random point to connect
                    var randomEndPoint = detectedMapPoint[Random.Range(0, detectedCount)] as MapPointCollection;

                    var nearestPoint = GetNearestPoint(startPoint, randomEndPoint);

                    if (nearestPoint == default)
                        continue;

                    //Make sure nothing block
                    var direction = nearestPoint.Item2.transform.position - nearestPoint.Item1.transform.position;

                    if (IsBlocked(nearestPoint.Item1.transform.position, nearestPoint.Item2.transform.position))
                        continue;

                    //CONNECT the point by generating the mesh
                    GenerateMesh(nearestPoint.Item1.transform.position, nearestPoint.Item2.transform.position, direction.magnitude);

                    //Store it
                    if (!MapGenData.buildingPath.ContainsKey(uniqueComponentID)) {

                        MapGenData.buildingPath.Add(uniqueComponentID, new List<PathPoint>());

                    }

                    MapGenData.buildingPath[uniqueComponentID].Add(new PathPoint 
                    { 
                        startPos = nearestPoint.Item1.transform.position, 
                        endPos = nearestPoint.Item2.transform.position 
                    });

                    break;
                }

            }

        }

        if (GameManagerBase.instance.isClient) {

            for(int i = 0; i < MapGenData.buildingPath[uniqueComponentID].Count; i++) {

                var pathData = MapGenData.buildingPath[uniqueComponentID][i];
                var direction = pathData.endPos - pathData.startPos;
                GenerateMesh(pathData.startPos, pathData.endPos, direction.magnitude);

            }

        }

        yield return null;

    }

    private int DetectedNearby(GameObject startPos, ref ArrayList nearbyPoints) {

        for (int i = 0; i < mapPointsCollection.Length; i++) {

            if (startPos.gameObject == mapPointsCollection[i].gameObject)
                continue;

            if(Vector3.Distance(startPos.transform.position, mapPointsCollection[i].transform.position) < maxConnectLegth) {

                //Find nearest point
                nearbyPoints.Add(mapPointsCollection[i]);

            }

        }

        return nearbyPoints.Count;
    }

    private (MapPoint, MapPoint) GetNearestPoint(MapPointCollection point1, MapPointCollection point2) {

        (MapPoint, MapPoint) nearestPoint = default;

        float distance = float.MaxValue;

        foreach(var pointA in point1.mapPoints) {

            if(pointA.ContainType(pointType)) {

                foreach(var pointB in point2.mapPoints) {

                    if(pointB.ContainType(pointType)) {

                        var cDistance = Vector3.Distance(pointA.transform.position, pointB.transform.position);

                        if(distance > cDistance) {

                            distance = cDistance;
                            nearestPoint.Item1 = pointA;
                            nearestPoint.Item2 = pointB;

                        }

                    }

                }

            }

        }

        return nearestPoint;

    }

    public abstract void GenerateMesh(Vector3 start, Vector3 end, float length);

    protected virtual bool IsBlocked(Vector3 startPos, Vector3 endPos) {

        var direction = endPos - startPos;
        Ray ray = new Ray(startPos, direction.normalized);

        return Physics.Raycast(ray, direction.magnitude, blockMask);

    }

    protected void CreateMeshInstance(Matrix4x4 transformMatrix, Mesh targetMesh, out CombineInstance combineInstance) {

        //Start of the bridge
        combineInstance = new CombineInstance();
        combineInstance.mesh = targetMesh;
        combineInstance.transform = transformMatrix;

    }
}
