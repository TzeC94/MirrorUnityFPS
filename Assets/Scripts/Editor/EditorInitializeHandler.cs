using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EditorInitializeHandler : Editor
{
    [MenuItem(EditorConstant.CustomPath + "Interface Initialize")]
    public static void SetupInitialize() {

        //Look for asset implement this
        var filteredAssetsPath = AssetDatabase.FindAssets("t:prefab");

        foreach (var assetPath in filteredAssetsPath) {

            var path = AssetDatabase.GUIDToAssetPath(assetPath);
            var loadedObject = AssetDatabase.LoadAssetAtPath(path, typeof(GameObject)) as GameObject;
            var editorInterface = loadedObject.GetComponent<IEditorInitialize>();
            if (editorInterface != null) {

                Debug.Log($"Initialize Gameobject {loadedObject.name}");
                editorInterface.OnEditorInitialize();

            }

        }

    }
}
