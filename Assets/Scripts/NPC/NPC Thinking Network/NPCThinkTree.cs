using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NPC Think Tree", menuName = "AI/NPC Think Tree")]
public class NPCThinkTree : ScriptableObject
{
    [Header("Node List")]
    public NPCThinkNode[] nodeList;
    private int nodeCount;

    [Header("Always")]
    public NPCThinkNode[] alwaysRunNode;
    private int alwaysNodeCount;

    private NPCThinkNode currentNode;
    private BaseNPC _currentNPC;
    public BaseNPC currentNPC { get { return _currentNPC; } }

    //Shared Data Store
    public Dictionary<string, System.Object> systemTypeSharedData = new Dictionary<string, System.Object>();
    public Dictionary<string, UnityEngine.Object> unityTypeSharedData = new Dictionary<string, UnityEngine.Object>();

    public void Initialize(BaseNPC thisNPC) {

        _currentNPC = thisNPC;

        currentNode = nodeList[0];
        currentNode.OnStart();

        //SETTTTT
        foreach(var thinkNode in nodeList) {
            thinkNode.Initialize(this);
        }

        foreach(var thinkNode in alwaysRunNode) {
            thinkNode.Initialize(this);
        }

    }

    public void Update() {

        CurrentNodeUpdate();

        AlwaysNodeUpdate();

    }

    private void CurrentNodeUpdate() {

        if (currentNode != null) {

            currentNode.OnUpdate();

        }

    }

    public void CurrentNodeEnded(int nextNodeIndex) {

        currentNode = null;

        //Find the next node
        var nextNode = nodeList[nextNodeIndex];
        nextNode.OnStart();

        currentNode = nextNode;

    }

    private void AlwaysNodeUpdate() {

        for (int i = 0; i < alwaysNodeCount; i++) {

            var currentAlwaysNode = alwaysRunNode[i];
            currentAlwaysNode.OnUpdate();

        }

    }

#if UNITY_EDITOR

    private void OnValidate() {

        nodeCount = nodeList.Length;
        alwaysNodeCount = alwaysRunNode.Length;

    }

#endif
}
