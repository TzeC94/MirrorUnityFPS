using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public static class ResourceManage
{
    
    public static void LoadAsset<T>(string address, Action<T> onComplete) {

        var handle = Addressables.LoadAssetAsync<T>(address);
        handle.Completed += (operation) => { 

            if(operation.Status == AsyncOperationStatus.Succeeded) {
                onComplete.Invoke(operation.Result);
            }
            
        };

    }

}
