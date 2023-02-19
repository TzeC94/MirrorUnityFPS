using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IDLE Node", menuName = "AI/IDLE Node")]
public class NPCIDLENode : NPCThinkNode {

#if UNITY_EDITOR
    public override void InitializeOutput() {

        outputNode = new NextNode[0];

    }
#endif

    protected override void OnFailed() {
        
    }

    public override void OnStart() {
        
    }

    public override void OnUpdate() {
        
    }
}
