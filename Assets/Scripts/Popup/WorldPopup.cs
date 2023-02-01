using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldPopup : MonoBehaviour {

    //Singleton
    private static WorldPopup _instance;
    public static WorldPopup instance { get { return _instance; } }

    [Header("Prefab")]
    public GameObject popupPrefab;

    // Start is called before the first frame update
    void Start()
    {
        if(_instance == null) {
            _instance = this;
        }
    }

    private void OnDisable() {

        _instance = null;

    }

    public void SpawnPopup(string message, Vector3 worldPosition) {

        var popupSpawned = GameCore.Instantiate(popupPrefab, worldPosition);
        var popupItem = popupSpawned.GetComponent<PopupItem>();
        if(popupItem != null) {

            //Lets face the camera
            popupSpawned.transform.LookAt(Camera.main.transform.position);

            popupItem.SetText(message);

        } else {

            Debug.LogError($"World Popup: Something wrong with this {popupSpawned.name} where PopupItem component is NULL");

        }

    }
    
}