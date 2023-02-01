using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Mirror;

public class ItemSetupEditor : Editor
{
    private const string networkManagerPath = "Assets/Prefab/Networking/Network Manager.prefab";

    [MenuItem(EditorConstant.CustomPath + "Item Data/Setup All Item Data")]
    private static void SetupItemsData() {

        //Need look for the network manager for the information
        var networkManager = AssetDatabase.LoadAssetAtPath<NetworkManager>(networkManagerPath);

        if(networkManager != null) {

            var registeredList = networkManager.spawnPrefabs;

            for(int i = 0; i < registeredList.Count; i++) {

                //Set the index if item data is available
                var pickupBase = registeredList[i].GetComponent<PickupBase>();

                if (pickupBase) {

                    var itemData = pickupBase.itemData.GetItemData();
                    itemData.itemIndex = i;

                    //SetDirty
                    EditorUtility.SetDirty(itemData);

                }

            }

        }else {

            Debug.LogError($"Can't Find Network Manager Prefab, is the path changed???\nCurrent Looking Up Path: {networkManagerPath}");

        }
    }
}
