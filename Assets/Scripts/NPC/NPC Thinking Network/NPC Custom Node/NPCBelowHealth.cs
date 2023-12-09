using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Below Health", menuName = "AI/Below Health")]
public class NPCBelowHealth : NPCThinkNode {

    public float healthBelowPercent = 50f;

    public override int outputNodeCount => 1;

#if UNITY_EDITOR

    protected override void Reset() {

        base.Reset();

        titleNameList[0] = "Below Health Hit";

    }

#endif

    protected override void OnStart() {
        
    }

    protected override void OnUpdate() {
        
        float healthThresold = myThinkTree.currentNPC.MaxHealth / 100f * healthBelowPercent;

        if(healthThresold >= myThinkTree.currentNPC.CurrentHealth) {

            EndNode(0);

        }

    }

    protected override void OnFailed() {
        
    }

    protected override void OnEnd() {
        
    }
}