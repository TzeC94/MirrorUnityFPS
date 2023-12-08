using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Below Health", menuName = "AI/Below Health")]
public class NPCBelowHealth : NPCThinkNode {

    public float healthBelowTarget = 50f;

    public override int outputNodeCount => 1;

#if UNITY_EDITOR

    protected override void OnValidate() {

        base.OnValidate();

        titleNameList[0] = "Below Health Hit";

    }

#endif

    protected override void OnStart() {
        
    }

    protected override void OnUpdate() {
        
        if(healthBelowTarget >= myThinkTree.currentNPC.CurrentHealth) {

            EndNode(0);

        }

    }

    protected override void OnFailed() {
        
    }

    protected override void OnEnd() {
        
    }
}