using Mirror;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract partial class GameManagerBase
{
    [Client]
    void LoadUI() {

        var loadMasterAsync = SceneManager.LoadSceneAsync("Master UI", LoadSceneMode.Additive);
        loadMasterAsync.allowSceneActivation = true;
        loadMasterAsync.completed += LoadMasterUICompleted;

    }

    [Client]
    void LoadMasterUICompleted(AsyncOperation asyncOperation) {

        var loadInventoryAynsc = SceneManager.LoadSceneAsync("Inventory UI", LoadSceneMode.Additive);
        loadInventoryAynsc.allowSceneActivation = true;
        loadInventoryAynsc.completed += LoadInventoryUICompleted;

    }

    [Client]
    void LoadInventoryUICompleted(AsyncOperation asyncOperation) {

        var uiObject = GameObject.Find("Inventory Panel");
        uiObject.transform.SetParent(MasterUIScript.instance.transform);

        playerInventoryUI = uiObject.GetComponent<PlayerInventoryUIScript>();
        playerInventoryUI.Init();
        playerInventoryUI.Close();

        //Unload this scene
        SceneManager.UnloadSceneAsync("Inventory UI", UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);

        //Load HUD
        LoadHUDUI();
    }

    [Client]
    void LoadHUDUI() {

        var hudUIAsync = SceneManager.LoadSceneAsync("HUD UI", LoadSceneMode.Additive);
        hudUIAsync.allowSceneActivation = true;
        hudUIAsync.completed += LoadHUDUICompleted;

    }

    [Client]
    void LoadHUDUICompleted(AsyncOperation asyncOperation) {

        var uiObject = GameObject.Find("HUD");
        uiObject.transform.SetParent(MasterUIScript.instance.transform);

        //Unload this scene
        SceneManager.UnloadSceneAsync("HUD UI", UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);

    }

    protected abstract IEnumerator ServerStartProcess();

    [Server]
    private IEnumerator ServerProcess() {

        yield return ServerStartProcess();

    }

    protected abstract IEnumerator ClientStartProcess();

    [Client]
    private IEnumerator ClientProcess() 
    {
        yield return ClientStartProcess();

        _ClientReady = true;
    }
}
