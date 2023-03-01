using System;
using UnityEditor;
using UnityEngine;

public abstract class NPCThinkNode : ScriptableObject
{
    public enum State {
        None,
        Success,
        Running,
        Failed
    }

    [NonSerialized]
    public State _state = State.None;

    protected NPCThinkTree myThinkTree;

    /// <summary>
    /// Need to override this
    /// </summary>
    public virtual int outputNodeCount { get; internal set; }

    public void Initialize(NPCThinkTree parentThinkTree) {

        myThinkTree = parentThinkTree;

    }

    public abstract void OnStart();

    protected virtual void OnEnd(int nextNodeIndex) {

        myThinkTree.CurrentNodeEnded(nextNodeIndex);

    }

    public abstract void OnUpdate();
    protected abstract void OnFailed();

}

[Serializable]
public class NextNode {

#if UNITY_EDITOR
    public string title;
#endif
    public int nextNodeIndex;

}