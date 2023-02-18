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

#if UNITY_EDITOR
    public override void InitializeOutput() {

        outputNode = new NextNode[1];
        
    }
#endif

    protected override void OnEnd() {

        base.OnEnd();

        myThinkTree.CurrentNodeEnded(outputNode[0].nextNodeIndex);

    }

    protected override void OnFailed() {

        base.OnFailed();

    }

    public override void OnStart() {

        base.OnStart();

    }

    public override void OnUpdate() {

        base.OnUpdate();

        //Scout for target
        var target = RayTracer.ObjectInFOV(myThinkTree.currentNPC.transform, distance, fov, detectionCollider, scoutLayerMask);

        if (target != null) {

            //Set the data
            if (!myThinkTree.unityTypeSharedData.ContainsKey(NPCHelper.targetString)) {

                myThinkTree.unityTypeSharedData.Add(NPCHelper.targetString, null);

            }

            myThinkTree.unityTypeSharedData[NPCHelper.targetString] = target;

            OnEnd();

        }

    }
}
