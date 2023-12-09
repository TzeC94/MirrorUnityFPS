using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IDLE Node", menuName = "AI/IDLE Node")]
public class NPCIDLENode : NPCThinkNode {

    public override int outputNodeCount => 1;

#if UNITY_EDITOR

    protected override void Reset() {

        base.Reset();

        titleNameList[0] = "IDLE Success";

    }

#endif

    protected override void OnFailed() {
        
    }

    protected override void OnStart() {
        
    }

    protected override void OnUpdate() {
        
    }

    protected override void OnEnd() {
        
    }
}
