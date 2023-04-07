using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NPC Think Tree", menuName = "AI/NPC Think Tree")]
public class NPCThinkTree : ScriptableObject
{
    [Header("Node List")]
    public NPCThinkTreeNodeDetail[] nodeList;
    private int nodeCount;

    [Header("Always")]
    public NPCThinkTreeNodeDetail[] alwaysRunNode;
    private int alwaysNodeCount;

    public NPCThinkTreeNodeDetail currentNode { get; private set; }
    private BaseNPC _currentNPC;
    public BaseNPC currentNPC { get { return _currentNPC; } }

    //Shared Data Store
    public Dictionary<string, System.Object> systemTypeSharedData = new Dictionary<string, System.Object>();
    public Dictionary<string, UnityEngine.Object> unityTypeSharedData = new Dictionary<string, UnityEngine.Object>();

    public void Initialize(BaseNPC thisNPC) {

        _currentNPC = thisNPC;

        currentNode = nodeList[0];
        currentNode.thinkNode.OnStart();

        //SETTTTT
        foreach(var thinkNode in nodeList) {
            thinkNode.thinkNode.Initialize(this);
        }

        foreach(var thinkNode in alwaysRunNode) {
            thinkNode.thinkNode.Initialize(this);
        }

    }

    public void Update() {

        CurrentNodeUpdate();

        AlwaysNodeUpdate();

    }

    private void CurrentNodeUpdate() {

        if (currentNode != null) {

            currentNode.thinkNode.OnUpdate();

        }

    }

    public void CurrentNodeEnded(int nextNodeIndex) {

        //Find the next node
        var nextNodeData = currentNode.nextNode[nextNodeIndex];
        currentNode = nodeList[nextNodeData.nextNodeIndex];
        currentNode.thinkNode.OnStart();

    }

    private void AlwaysNodeUpdate() {

        for (int i = 0; i < alwaysNodeCount; i++) {

            var currentAlwaysNode = alwaysRunNode[i];
            currentAlwaysNode.thinkNode.OnUpdate();

        }

    }

#if UNITY_EDITOR

    private void OnValidate() {

        nodeCount = nodeList.Length;
        alwaysNodeCount = alwaysRunNode.Length;

        //Match the node list
        InitializeOutputNodeArray(nodeList);
        InitializeOutputNodeArray(alwaysRunNode);

    }

    private void InitializeOutputNodeArray(NPCThinkTreeNodeDetail[] thinkTreeArray) {

        for (int i = 0; i < thinkTreeArray.Length; i++) {

            var currentNodeDetail = thinkTreeArray[i];
            if (currentNodeDetail.thinkNode) {

                var currentNodeData = currentNodeDetail.thinkNode;

                if (currentNodeData.outputNodeCount != currentNodeDetail.nextNode.Length) {

                    currentNodeDetail.nextNode = new NextNode[currentNodeData.outputNodeCount];

                    for(int j = 0; j < currentNodeData.titleNameList.Length; j++) {

                        var newNextNode = new NextNode();
                        newNextNode.title = currentNodeData.titleNameList[j];
                        currentNodeDetail.nextNode[j] = newNextNode;

                    }

                }

            } else {

                currentNodeDetail.nextNode = new NextNode[0];

            }

        }

    }

#endif
}