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

    protected override void OnEnd() {
        throw new System.NotImplementedException();
    }

    protected override void OnFailed() {
        throw new System.NotImplementedException();
    }

    public override void OnStart() {
        throw new System.NotImplementedException();
    }

    public override void OnUpdate() {
        throw new System.NotImplementedException();
    }
}
