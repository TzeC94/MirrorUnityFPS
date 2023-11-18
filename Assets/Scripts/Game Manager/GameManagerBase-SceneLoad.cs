using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public partial class GameManagerBase
{
    void LoadUI() {

        var loadMasterAsync = SceneManager.LoadSceneAsync("Master UI", LoadSceneMode.Additive);
        loadMasterAsync.allowSceneActivation = true;
        loadMasterAsync.completed += LoadMasterUICompleted;

    }

    void LoadMasterUICompleted(AsyncOperation asyncOperation) {

        var loadInventoryAynsc = SceneManager.LoadSceneAsync("Inventory UI", LoadSceneMode.Additive);
        loadInventoryAynsc.allowSceneActivation = true;
        loadInventoryAynsc.completed += LoadInventoryUICompleted;

    }

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

    void LoadHUDUI() {

        var hudUIAsync = SceneManager.LoadSceneAsync("HUD UI", LoadSceneMode.Additive);
        hudUIAsync.allowSceneActivation = true;
        hudUIAsync.completed += LoadHUDUICompleted;

    }

    void LoadHUDUICompleted(AsyncOperation asyncOperation) {

        var uiObject = GameObject.Find("HUD");
        uiObject.transform.SetParent(MasterUIScript.instance.transform);

        //Unload this scene
        SceneManager.UnloadSceneAsync("HUD UI", UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);

    }

    protected virtual IEnumerator ServerStartProcess() 
    {
        yield return null;
    }

    protected virtual IEnumerator ClientStartProcess() 
    {
        yield return null;

        _ClientReady = true;
    }
}
