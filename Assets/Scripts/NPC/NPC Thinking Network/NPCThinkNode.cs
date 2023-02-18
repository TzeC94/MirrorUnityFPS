using System;
using System.Collections;
using System.Collections.Generic;
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

    public void Initialize(NPCThinkTree parentThinkTree) {

        myThinkTree = parentThinkTree;

    }

    public abstract void OnStart();
    protected abstract void OnEnd();
    public abstract void OnUpdate();
    protected abstract void OnFailed();

    /// <summary>
    /// Need to override this
    /// </summary>
    public NextNode[] outputNode;

#if UNITY_EDITOR

    bool initialized = false;

    public virtual void OnValidate() {

        if(initialized == false)
            InitializeOutput();

        initialized = true;

    }

    /// <summary>
    /// Use this to initialize the number of node
    /// </summary>
    public abstract void InitializeOutput();

#endif

}

[System.Serializable]
public class NextNode {

#if UNITY_EDITOR
    public string title;
#endif
    public int nextNodeIndex;

}