using UnityEngine;
using Mirror;

public abstract partial class GameManagerBase : NetworkBehaviour
{
    public static GameManagerBase instance;

    public static PlayerBase LocalPlayer;

    private static bool _ClientReady = false;

    //Make sure we set this at the end of it when we're really ready
    public static bool ClientReady { get { return _ClientReady; } }

    PlayerInventoryUIScript playerInventoryUI;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        if(instance == null) {

            instance = this;

        } else {

            return;

        }

        if (isClient){

            LoadUI();
            StartCoroutine(ClientProcess());

        }

        if (isServer) {

            StartCoroutine(ServerProcess());

        }
    }

    protected virtual void OnDisable() {

        instance = null;

    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }

    #region Pick up

    [Client]
    public void PickupItemToInventory(uint itemNetID, uint targetNetID) {

        CmdPickupItemToInventory(itemNetID, targetNetID);

    }

    [Command(requiresAuthority = false)]
    private void CmdPickupItemToInventory(uint itemNetID, uint targetNetID) {

        NetworkIdentity targetNetworkIdentity;

        if (NetworkServer.spawned.TryGetValue(itemNetID, out targetNetworkIdentity)) {

            var itemData =  targetNetworkIdentity.gameObject.GetComponent<PickupBase>().itemData;
            AddItemToInventory(itemData, targetNetID);

            //Destroy this object from server
            NetworkServer.Destroy(targetNetworkIdentity.gameObject);
        }

    }

    [Server]
    public static void AddItemToInventory(Item item, uint targetNetID) {

        NetworkIdentity targetNetworkIdentity;

        if (NetworkServer.spawned.TryGetValue(targetNetID, out targetNetworkIdentity)) {

            var playerInventory = targetNetworkIdentity.GetComponent<PlayerInventory>();

            //Add
            playerInventory.AddToInventory(item);

        }

    }

    #endregion

    #region Drop

    [Client]
    public void DropItemFromInventory(int inventoryIndex, Item item, uint targetNetID, InventoryBase.InventoryType inventoryType) {

        CmdDropItemFromInventory(inventoryIndex, item.GetItemData().itemIndex, targetNetID, inventoryType);

    }

    [Command(requiresAuthority = false)]
    private void CmdDropItemFromInventory(int inventoryIndex, int itemIndex, uint targetNetID, InventoryBase.InventoryType inventoryType) {

        //Remove from player inventory
        NetworkIdentity targetNetworkIdentity;

        if (NetworkServer.spawned.TryGetValue(targetNetID, out targetNetworkIdentity)) {

            var inventory = targetNetworkIdentity.GetComponents<InventoryBase>();

            for(int i = 0; i < inventory.Length; i++) {

                var cInventory = inventory[i];

                if (cInventory.inventoryType != inventoryType)
                    continue;

                //Get the Item Data first
                var item = cInventory.FindItemInInventory(inventoryIndex);

                if (item != null) {

                    //Remove and success
                    if (cInventory.RemoveFromInventory(inventoryIndex)) {

                        //Spawn the item to the world
                        var playerPosFront = targetNetworkIdentity.transform.position + targetNetworkIdentity.transform.forward + Vector3.up;
                        var toSpawnPrefab = MyNetworkManager.instance.spawnPrefabs[itemIndex];
                        GameCore.DropItemNetworkInstantiate(toSpawnPrefab, playerPosFront, item);

                        break;

                    }

                }

            }

        }
    }

    #endregion

}
