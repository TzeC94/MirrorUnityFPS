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

    public void Initialize(NPCThinkTree parentThinkTree) {

        myThinkTree = parentThinkTree;

    }

    public abstract void OnStart();

    protected virtual void OnEnd(int nextNodeIndex) {

        myThinkTree.CurrentNodeEnded(nextNodeIndex);

    }

    public abstract void OnUpdate();
    protected abstract void OnFailed();

    /// <summary>
    /// Need to override this
    /// </summary>
    public NextNode[] outputNode;

#if UNITY_EDITOR

    [HideInInspector]
    public bool initialized = false;

    public virtual void OnValidate() {

        if(initialized == false) {

            InitializeOutput();
            initialized = true;

            EditorUtility.SetDirty(this);

        }

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