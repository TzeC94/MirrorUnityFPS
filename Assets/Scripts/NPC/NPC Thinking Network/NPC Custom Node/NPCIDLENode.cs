using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IDLE Node", menuName = "AI/IDLE Node")]
public class NPCIDLENode : NPCThinkNode {

    public override int outputNodeCount => 1;

#if UNITY_EDITOR

    protected override void OnValidate() {

        base.OnValidate();

        titleNameList[0] = "IDLE Success";

    }

#endif

    protected override void OnFailed() {
        
    }

    public override void OnStart() {
        
    }

    public override void OnUpdate() {
        
    }
}
