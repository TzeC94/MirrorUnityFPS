using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Walk Node", menuName = "AI/Walk Node")]
public class NPCWalkNode : NPCThinkNode {

    private GameObject target;

#if UNITY_EDITOR
    public override void InitializeOutput() {

        outputNode = new NextNode[2];

    }
#endif

    protected override void OnFailed() {
        throw new System.NotImplementedException();
    }

    public override void OnStart() {

        //Grab my target
        target = myThinkTree.unityTypeSharedData[NPCHelper.targetString] as GameObject;

        //Set my target to this destination
        myThinkTree.currentNPC.MoveTo(target.transform.position);

    }

    public override void OnUpdate() {

        if (myThinkTree.currentNPC.CloseToTarget()) {

            OnEnd(0);

        }

    }
}
