using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MapBuildingBridgeGeneration : BaseMapBuildingPathGen
{
    [Header("Bridge")]
    public Mesh bridgeMesh;
    public float bridgeSize = 6f;
    public Mesh bridgeStart;
    public Mesh bridgeEnd;
    public Material bridgeMaterial;

    public override void GenerateMesh(Vector3 start, Vector3 end, float length) {

        if (length - bridgeSize < bridgeSize)
            return;

        GameObject bridgeObject = new GameObject("Bridge");
        //bridgeObject.transform.position = start;

        List<CombineInstance> combineInstance = new List<CombineInstance>();

        //Start of the bridge
        CombineInstance combineMesh = new CombineInstance();
        combineMesh.mesh = bridgeStart;
        combineMesh.transform = bridgeObject.transform.localToWorldMatrix;
        combineInstance.Add(combineMesh);

        //Loop of the bridge
        //Add the remainding till the end
        var startDistance = bridgeSize;

        while (startDistance <= length - bridgeSize) {

            combineMesh = new CombineInstance();
            combineMesh.mesh = bridgeMesh;
            Matrix4x4 movedPoint = Matrix4x4.Translate(startDistance * bridgeObject.transform.forward);
            combineMesh.transform = movedPoint * bridgeObject.transform.localToWorldMatrix;
            combineInstance.Add(combineMesh);

            startDistance += bridgeSize;

        }

        //End of the bridge
        combineMesh = new CombineInstance();
        combineMesh.mesh = bridgeEnd;
        combineMesh.transform = Matrix4x4.Translate(startDistance * bridgeObject.transform.forward) * bridgeObject.transform.localToWorldMatrix;
        combineInstance.Add(combineMesh);

        var meshFilter = bridgeObject.AddComponent<MeshFilter>();
        Mesh finalMesh = new Mesh();
        finalMesh.CombineMeshes(combineInstance.ToArray());
        meshFilter.mesh = finalMesh;

        var meshRenderer = bridgeObject.AddComponent<MeshRenderer>();
        meshRenderer.sharedMaterial = bridgeMaterial;

        //Look at that
        bridgeObject.transform.position = start;
        bridgeObject.transform.LookAt(end);

    }
}