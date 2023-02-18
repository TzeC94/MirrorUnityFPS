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

    public abstract void OnStart();
    public abstract void OnEnd();
    public abstract void OnUpdate();
    public abstract void OnFailed();

    /// <summary>
    /// Need to override this
    /// </summary>
    public NextNode[] outputNode;

#if UNITY_EDITOR
    public virtual void OnValidate() {

        InitializeOutput();

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