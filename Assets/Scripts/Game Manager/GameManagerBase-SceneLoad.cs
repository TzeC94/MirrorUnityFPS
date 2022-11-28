using System.Collections;
using System.Collections.Generic;
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
    }
}
