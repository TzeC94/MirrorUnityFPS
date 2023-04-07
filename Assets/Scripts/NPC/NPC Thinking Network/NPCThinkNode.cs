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

#if UNITY_EDITOR

    public string[] titleNameList = new string[0];

    protected virtual void OnValidate() {
        
        if(titleNameList.Length != outputNodeCount) {

            titleNameList = new string[outputNodeCount];

        }

    }

#endif

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

    public NextNode() {

    }

#if UNITY_EDITOR
    public string title;
#endif
    public int nextNodeIndex;

}