using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public abstract class BaseMapBuildingPathGen : MapGenComponent {

    public int generationTryCount = 10;
    public int maxConnectLegth = 10;
    public MapPoint.PointType pointType;
    public LayerMask blockMask;

    private MapPoint[] mapPoints;

    public override void Initialize() {

        base.Initialize();

        //Look for the point
        mapPoints = GameObject.FindObjectsOfType<MapPoint>(true).
            Where(t => ((int)t.pointType & 1 << (int)pointType) == (int)t.pointType).
            ToArray();

    }

    public override IEnumerator Process() {

        if (mapPoints.Length == 0)
            yield break;

        List<MapPoint> detectedMapPoint = new List<MapPoint>();

        //Loop to try to spawn
        for (int i = 0; i < generationTryCount; i++) {

            //Look for random point
            var index = Random.Range(0, mapPoints.Length);

            var startPoint = mapPoints[index];

            //Look for nearby point
            detectedMapPoint.Clear();   //Make sure we clear first
            int detectedCount = DetectedNearby(startPoint.gameObject, detectedMapPoint);

            if (detectedCount <= 0)
                continue;

            //Try connect within this range of point
            for(int id = 0; id < detectedCount; id++) {

                //Find a random point to connect
                var randomEndPoint = detectedMapPoint[Random.Range(0, detectedCount)];

                //Make sure nothing block
                var direction = randomEndPoint.transform.position - startPoint.transform.position;
                Ray ray = new Ray(startPoint.transform.position, direction.normalized);

                //TODO add the layermask check here
                if (Physics.Raycast(ray, direction.magnitude, blockMask))
                    continue;

                //CONNECT the point by generating the mesh
                GenerateMesh(startPoint.transform.position, randomEndPoint.transform.position, direction.magnitude);

                break;
            }

            yield return null;

        }

        yield return null;

    }

    private int DetectedNearby(GameObject startPos, List<MapPoint> nearbyPoints) {

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
