using Mirror;
using System.Collections;

public class PlayerInventory : InventoryBase
{
    //Singleton
    public static PlayerInventory instance;

    public ItemData[] defaultInventoryItem;

    // Start is called before the first frame update
    public override void OnStartLocalPlayer() {

        if (instance == null) {

            instance = this;

        }

        base.OnStartLocalPlayer();
    }

    public override IEnumerator InitializeUI(){

        while(PlayerInventoryUIScript.instance == null){

            yield return null;

        }

        PlayerInventoryUIScript.instance.InitializeInventory(inventoryMax, this);

    }

    private void OnDestroy() {

        instance = null;

    }

    public override void OnInventoryChanged(SyncList<Item>.Operation op, int itemIndex, Item oldItem, Item newItem) {

        //IF the UI is open, then we need to refresh it
        if (isLocalPlayer && PlayerInventoryUIScript.instance != null && PlayerInventoryUIScript.instance.isOpen) {

            PlayerInventoryUIScript.instance.RefreshItem();

        }

    }

    public override void InitializeCollectedItem() {

        base.InitializeCollectedItem();

        foreach(var item in defaultInventoryItem) {

            AddToInventory(new Item(item));

        }

    }

}