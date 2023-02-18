using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NPC Think Tree", menuName = "AI/NPC Think Tree")]
public class NPCThinkTree : ScriptableObject
{
    [Header("Root")]
    public NPCThinkNode rootNode;

    [Header("Node List")]
    public NPCThinkNode[] nodeList;
    private int nodeCount;

    [Header("Always")]
    public NPCThinkNode[] alwaysRunNode;
    private int alwaysNodeCount;

    private NPCThinkNode currentNode;

    public void Initialize() {

        currentNode = rootNode;
        currentNode.OnStart();

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
