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
                if(loadHandle.Result != null) {
                    //Debug.Log($"Add Asset {location.PrimaryKey}", loadHandle.Result);
                    preloadedGameplayAssets.Add(location.PrimaryKey, loadHandle.Result);
                } else {
                    Debug.LogError($"Item load at {location.PrimaryKey} is null");
                }
                
            };

        }

    }

    public static T GetPreloadGameplayData<T>(string address) where T : ScriptableObject {

        if (preloadedGameplayAssets.ContainsKey(address)) {
            return (T)preloadedGameplayAssets[address];
        } else {
            Debug.LogError($"Item at {address} is null");
            return null;
        }
        

    }

}