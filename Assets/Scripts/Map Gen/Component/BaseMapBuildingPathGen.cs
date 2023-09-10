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

    private MapPoint[] mapPoints;

    public struct PathPoint {
        public Vector3 startPos;
        public Vector3 endPos;
    }

    public override void Initialize() {

        base.Initialize();

        if (GameManagerBase.instance.isServer) {

            Random.InitState(MapGenData.seed);

            //Look for the point
            mapPoints = GameObject.FindObjectsOfType<MapPoint>(true).
                Where(t => ((int)t.pointType & (int)pointType) == 1).
                ToArray();

        }

    }

    public override IEnumerator Process() {

        if (GameManagerBase.instance.isServer) {

            if (mapPoints.Length == 0)
                yield break;

            ArrayList detectedMapPoint = new ArrayList();

            //Loop to try to spawn
            for (int i = 0; i < generationTryCount; i++) {

                //Look for random point
                var index = Random.Range(0, mapPoints.Length);

                var startPoint = mapPoints[index];

                //Look for nearby point
                detectedMapPoint.Clear();   //Make sure we clear first
                int detectedCount = DetectedNearby(startPoint.gameObject, ref detectedMapPoint);

                if (detectedCount <= 0)
                    continue;

                //Try connect within this range of point
                for (int id = 0; id < detectedCount; id++) {

                    //Find a random point to connect
                    var randomEndPoint = detectedMapPoint[Random.Range(0, detectedCount)] as MapPoint;

                    //Make sure nothing block
                    var direction = randomEndPoint.transform.position - startPoint.transform.position;
                    Ray ray = new Ray(startPoint.transform.position, direction.normalized);

                    //TODO add the layermask check here
                    if (Physics.Raycast(ray, direction.magnitude, blockMask))
                        continue;

                    //CONNECT the point by generating the mesh
                    GenerateMesh(startPoint.transform.position, randomEndPoint.transform.position, direction.magnitude);

                    //Store it
                    if (!MapGenData.buildingPath.ContainsKey(uniqueComponentID)) {

                        MapGenData.buildingPath.Add(uniqueComponentID, new List<PathPoint>());

                    }

                    MapGenData.buildingPath[uniqueComponentID].Add(new PathPoint 
                    { 
                        startPos = startPoint.transform.position, 
                        endPos = randomEndPoint.transform.position 
                    });

                    break;
                }

                yield return null;

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

        for (int i = 0; i < mapPoints.Length; i++) {

            if (startPos.gameObject == mapPoints[i].gameObject)
                continue;

            if(Vector3.Distance(startPos.transform.position, mapPoints[i].transform.position) < maxConnectLegth) {

                nearbyPoints.Add(mapPoints[i]);

            }

        }

        return nearbyPoints.Count;
    }

    public abstract void GenerateMesh(Vector3 start, Vector3 end, float length);

    protected void CreateMeshInstance(Matrix4x4 transformMatrix, Mesh targetMesh, out CombineInstance combineInstance) {

        //Start of the bridge
        combineInstance = new CombineInstance();
        combineInstance.mesh = targetMesh;
        combineInstance.transform = transformMatrix;

    }
}
