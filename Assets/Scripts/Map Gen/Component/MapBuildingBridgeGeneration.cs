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

        List<CombineInstance> combineInstance = new List<CombineInstance>();

        //Start of the bridge
        CombineInstance combineMesh;
        CreateMeshInstance(bridgeObject.transform.localToWorldMatrix, bridgeStart, out combineMesh);
        combineInstance.Add(combineMesh);

        //Loop of the bridge
        //Add the remainding till the end
        var startDistance = bridgeSize;
        Matrix4x4 movedPoint;

        while (startDistance <= length - bridgeSize / 2f) {

            movedPoint = Matrix4x4.Translate(startDistance * bridgeObject.transform.forward) * bridgeObject.transform.localToWorldMatrix;
            CreateMeshInstance(movedPoint, bridgeMesh, out combineMesh);
            combineInstance.Add(combineMesh);

            startDistance += bridgeSize;

        }

        //End of the bridge
        movedPoint = Matrix4x4.Translate(startDistance * bridgeObject.transform.forward) * bridgeObject.transform.localToWorldMatrix;
        CreateMeshInstance(movedPoint, bridgeEnd, out combineMesh);

        combineInstance.Add(combineMesh);

        //Generate the real mesh here
        var meshFilter = bridgeObject.AddComponent<MeshFilter>();
        Mesh finalMesh = new Mesh();
        finalMesh.CombineMeshes(combineInstance.ToArray()); //Unity API to combine mesh
        meshFilter.mesh = finalMesh;

        var meshRenderer = bridgeObject.AddComponent<MeshRenderer>();
        meshRenderer.sharedMaterial = bridgeMaterial;

        //Generate the collider
        var meshCollider = bridgeObject.AddComponent<MeshCollider>();
        meshCollider.sharedMesh = finalMesh;
        //meshCollider.convex = true;

        //Look at that
        bridgeObject.transform.position = start;
        bridgeObject.transform.LookAt(end);

        //Force a physics sync, so then the blocking will work correctly
        Physics.SyncTransforms();
    }

    protected override bool IsBlocked(Vector3 startPos, Vector3 endPos) {

        //Block Check
        var hitCollider = RayTracer.OverlapSphere(startPos, 0.1f, blockMask);
        if (hitCollider != null) {
            return true;
        }

        hitCollider = RayTracer.OverlapSphere(endPos, 0.1f, blockMask);
        if (hitCollider != null) {
            return true;
        }

        var direction = endPos - startPos;
        Ray ray = new Ray(startPos, direction.normalized);
        if (Physics.CapsuleCast(startPos, startPos + Vector3.up, 0.1f, direction, direction.magnitude, blockMask))
            return true;

        return false;
    }

}