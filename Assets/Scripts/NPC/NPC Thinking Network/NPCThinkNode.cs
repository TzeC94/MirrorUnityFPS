using System;
using UnityEditor;
using UnityEngine;

public abstract class NPCThinkNode : ScriptableObject
{
    public bool StopRunAfterDone = false;
    private bool stopRun = false;

    protected NPCThinkTree myThinkTree;

    /// <summary>
    /// Need to override this
    /// </summary>
    public virtual int outputNodeCount { get; internal set; }

#if UNITY_EDITOR

    public string[] titleNameList = new string[0];

    protected virtual void Reset() {

        if (titleNameList.Length != outputNodeCount) {

            titleNameList = new string[outputNodeCount];

        }

    }

#endif

    public void Initialize(NPCThinkTree parentThinkTree) {

        myThinkTree = parentThinkTree;

    }

    protected abstract void OnStart();

    protected abstract void OnEnd();

    protected abstract void OnUpdate();

    protected abstract void OnFailed();

    public void StartNode() {

        OnStart();

    }

    public void UpdateNode() {
        
        if(stopRun) 
            return;

        OnUpdate();

    }

    public void EndNode(int nextNodeIndex) {

        OnEnd();

        myThinkTree.CurrentNodeEnded(nextNodeIndex);

        if (StopRunAfterDone) {
            stopRun = true;
        }

    }

    public void FailNode() {

        OnFailed();

        if (StopRunAfterDone) {
            stopRun = true;
        }

    }

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