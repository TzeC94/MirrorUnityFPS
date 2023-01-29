using MyBox.EditorTools;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

public static class ResourceManage
{
    private const string gameplayPreloadScriptableAssetPath = "Item Data";
    public static Dictionary<string, ScriptableObject> preloadedGameplayAssets = new Dictionary<string, ScriptableObject>();
    
    public static void LoadAsset<T>(string address, Action<T> onComplete) {

        var handle = Addressables.LoadAssetAsync<T>(address);
        handle.Completed += (operation) => { 

            if(operation.Status == AsyncOperationStatus.Succeeded) {
                onComplete.Invoke(operation.Result);
            }
            
        };

    }

    public static void UnloadAsset<T>(T target) {

        Addressables.Release<T>(target);

    }

    public static void PreloadRequiredGameplayAsset() {

        var locationHandle = Addressables.LoadResourceLocationsAsync(gameplayPreloadScriptableAssetPath, typeof(ScriptableObject));
        locationHandle.Completed += PreloadRequireGameplayAssetComplete;

    }

    private static void PreloadRequireGameplayAssetComplete(AsyncOperationHandle<IList<IResourceLocation>> handle) {

        foreach (var location in handle.Result) {

            var loadHandle = Addressables.LoadAssetAsync<ScriptableObject>(location);
            loadHandle.Completed += operation => {
                preloadedGameplayAssets.Add(location.PrimaryKey, loadHandle.Result);
            };

        }

    }

}