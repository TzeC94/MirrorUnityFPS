using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}