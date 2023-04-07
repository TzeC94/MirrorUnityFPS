using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public partial class BaseNPC 
{
    [Header("Think Tree")]
    public NPCThinkTree npcThinkTree;

    private void InitializeThinkTree() {

        npcThinkTree.Initialize(this);

    }

    private void ThinkTreeUpdate() {

        if(npcThinkTree != null) {
            npcThinkTree.Update();
        }

    }

#if UNITY_EDITOR

    public void OnDrawGizmosSelected() {

        if(Application.isPlaying) {

            Handles.color = Color.red;
            var textToDraw = "Current Think Node: " + npcThinkTree.currentNode.thinkNode.GetType().ToString();
            Handles.Label(transform.position + Vector3.up * 2f, textToDraw);

        }

    }

#endif

}
