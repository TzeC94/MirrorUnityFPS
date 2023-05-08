using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

public partial class MainMenuScript 
{
    [Header("Initial Loading")]
    [SerializeField] private CanvasGroup initialLoadingPanel;

    private void LoadInitialResources() {

        //Active the load
        initialLoadingPanel.alpha = 1f;

        //Load the Require Item Data
        ResourceManage.PreloadRequiredGameplayAsset(InitialLoadComplete);

    }

    private void InitialLoadComplete(AsyncOperationHandle<IList<IResourceLocation>> handle) {

        //Diable
        initialLoadingPanel.alpha = 0;

    }

}