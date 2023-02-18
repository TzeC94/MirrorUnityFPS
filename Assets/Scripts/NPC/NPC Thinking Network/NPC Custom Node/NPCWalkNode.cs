using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Walk Node", menuName = "AI/Walk Node")]
public class NPCWalkNode : NPCThinkNode {

#if UNITY_EDITOR
    public override void InitializeOutput() {

        outputNode = new NextNode[2];

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
