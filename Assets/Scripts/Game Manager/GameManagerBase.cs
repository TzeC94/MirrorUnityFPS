using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;

public class GameManagerBase : NetworkBehaviour
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

    void LoadUI(){

        var loadMasterAsync = SceneManager.LoadSceneAsync("Master UI", LoadSceneMode.Additive);
        loadMasterAsync.allowSceneActivation = true;
        loadMasterAsync.completed += LoadMasterUICompleted;

    }

    void LoadMasterUICompleted(AsyncOperation asyncOperation) {

        var loadInventoryAynsc = SceneManager.LoadSceneAsync("Inventory UI", LoadSceneMode.Additive);
        loadInventoryAynsc.allowSceneActivation = true;
        loadInventoryAynsc.completed += LoadInventoryUICompleted;

    }

    void LoadInventoryUICompleted(AsyncOperation asyncOperation){

        var uiObject = GameObject.Find("Inventory Panel");
        uiObject.transform.SetParent(MasterUIScript.instance.transform);

        playerInventoryUI = uiObject.GetComponent<PlayerInventoryUIScript>();
        playerInventoryUI.Init();
        playerInventoryUI.Close();

        //Unload this scene
        SceneManager.UnloadSceneAsync("Inventory UI", UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
    }

    #region Pick up

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

    public static void AddItemToInventory(ItemData itemData, uint targetNetID) {

        NetworkIdentity targetNetworkIdentity;

        if (NetworkServer.spawned.TryGetValue(targetNetID, out targetNetworkIdentity)) {

            var playerInventory = targetNetworkIdentity.GetComponent<PlayerInventory>();

            //Add
            playerInventory.AddToInventory(itemData);

        }

    }

    #endregion

}
