using UnityEngine;

[CreateAssetMenu(fileName = "Swap Held Item Node", menuName = "AI/Swap Held Item Node")]
public class NPCChangeHeldItemNode : NPCThinkNode {

    public override int outputNodeCount => 1;

    public int targetIndex = 0;

#if UNITY_EDITOR

    protected override void Reset() {

        base.Reset();

        titleNameList[0] = "Swap Item Done";

    }

#endif

    protected override void OnEnd() {
        
    }

    protected override void OnFailed() {
        
    }

    protected override void OnStart() {

        var npcInventory = myThinkTree.currentNPC.inventory;
        var npcHeld = myThinkTree.currentNPC.playerHeld;

        const int DEFAULT_HELD_INDEX = 0;
        npcHeld.CmdMoveItemFromToInventory(DEFAULT_HELD_INDEX, myThinkTree.currentNPC.netId,
            npcInventory.inventoryType, targetIndex);

        EndNode(0);

    }

    protected override void OnUpdate() {
        
    }
}
