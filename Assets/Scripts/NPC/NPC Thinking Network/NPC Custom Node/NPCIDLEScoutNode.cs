using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IDLE Scout Node", menuName = "AI/IDLE Scout Node")]
public class NPCIDLEScoutNode : NPCIDLENode
{
    public static Collider[] detectionCollider = new Collider[32];

    [Header("Scout")]
    public LayerMask scoutLayerMask;
    public float fov = 60f;
    public float distance = 20f;

    public override int outputNodeCount => 1;

#if UNITY_EDITOR

    protected override void OnValidate() {

        base.OnValidate();

        titleNameList[0] = "Scout Success";

    }

#endif

    protected override void OnFailed() {

        base.OnFailed();

    }

    protected override void OnStart() {

        base.OnStart();

    }

    protected override void OnUpdate() {

        base.OnUpdate();

        //Scout for target
        var target = RayTracer.ObjectInFOV(myThinkTree.currentNPC.transform, distance, fov, detectionCollider, scoutLayerMask);

        if (target != null) {

            //Set the data
            if (!myThinkTree.unityTypeSharedData.ContainsKey(NPCHelper.targetString)) {

                myThinkTree.unityTypeSharedData.Add(NPCHelper.targetString, null);

            }

            myThinkTree.unityTypeSharedData[NPCHelper.targetString] = target;

            EndNode(0);

        }

    }
}
