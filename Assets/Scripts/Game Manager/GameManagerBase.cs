using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;

public partial class GameManagerBase : NetworkBehaviour
{
    public static GameManagerBase instance;

    public static PlayerBase LocalPlayer;

    PlayerInventoryUIScript playerInventoryUI;

    // Start is called before the first frame update
    void Start()
    {
        if(instance == null) {
            instance = this;
        }

        if (isClient){

            LoadUI();

        }
    }

    private void OnDisable() {

        instance = null;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region Pick up

    //Call on Client
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

    //Server Code
    public static void AddItemToInventory(ItemData itemData, uint targetNetID) {

        NetworkIdentity targetNetworkIdentity;

        if (NetworkServer.spawned.TryGetValue(targetNetID, out targetNetworkIdentity)) {

            var playerInventory = targetNetworkIdentity.GetComponent<PlayerInventory>();

            //Add
            playerInventory.AddToInventory(itemData);

        }

    }

    #endregion

    #region Drop

    //Client Code
    public void DropItemFromInventory(int inventoryIndex, ItemData itemData, uint targetNetID) {

        CmdDropItemFromInventory(inventoryIndex, itemData.itemIndex, targetNetID);

    }

    [Command(requiresAuthority = false)]
    public void CmdDropItemFromInventory(int inventoryIndex, int itemIndex, uint targetNetID) {

        //Remove from player inventory
        NetworkIdentity targetNetworkIdentity;

        if (NetworkServer.spawned.TryGetValue(targetNetID, out targetNetworkIdentity)) {

            var playerInventory = targetNetworkIdentity.GetComponent<PlayerInventory>();

            //Remove and success
            if(playerInventory.RemoveFromInventory(inventoryIndex)) {

                //Spawn the item to the world
                var playerPosFront = targetNetworkIdentity.transform.position + targetNetworkIdentity.transform.forward + Vector3.up;
                var toSpawnPrefab = MyNetworkManager.instance.spawnPrefabs[itemIndex];
                GameCore.NetworkInstantiate(toSpawnPrefab, playerPosFront);

            }

        }
    }

    #endregion

}
