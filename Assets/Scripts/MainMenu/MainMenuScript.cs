using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;

public partial class MainMenuScript : MonoBehaviour
{

    [Header("Client Join")]
    [SerializeField] private TMP_InputField client_IPInputField;
    
    public void Server_Start(){

        MyNetworkManager.instance.StartServer();

    }

    public void Client_Join(){

        MyNetworkManager.instance.networkAddress = client_IPInputField.text;
        MyNetworkManager.instance.StartClient();

    }

}