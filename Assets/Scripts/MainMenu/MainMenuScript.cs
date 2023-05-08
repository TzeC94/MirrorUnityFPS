using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;

public partial class MainMenuScript : MonoBehaviour
{

    [Header("Client Join")]
    [SerializeField] private TMP_InputField client_IPInputField;

    private IEnumerator Start() {

        //Load the Require Item Data
        LoadInitialResources();

        yield return new WaitForSeconds(1f);

        //Start Server IF is server
        if (GlobalVar.isServerBuild) {

            Server_Start();

        }

#if UNITY_EDITOR
        if (ParrelSync.ClonesManager.IsClone()) {

            Server_Start();

        }
#endif
    }

    public void Server_Start(){

        MyNetworkManager.instance.StartServer();

    }

    public void Client_Join(){

        MyNetworkManager.instance.networkAddress = client_IPInputField.text;
        MyNetworkManager.instance.StartClient();

    }

}