using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GameManagerProcBase : GameManagerBase
{
    [Header("Map Generation")]
    public MapGenManager mapGenManager;

    //Cached map data
    byte[] cachedMapData;

    //Client
    bool clientReceivedMapData = false;

    protected override void Start() {

        base.Start();

        if (isServer) {

            StartCoroutine(ServerStartProcess());

        }

        if (isClient) {

            StartCoroutine(ClientStartProcess());

        }

    }

    [Server]
    private IEnumerator ServerStartProcess() {

        yield return mapGenManager.GenerationProcess();

        yield return null;

    }

    [Command(requiresAuthority = false)]
    protected void CmdClientJoinRequest(NetworkConnectionToClient sender = null) {

        //Gather the byte data that need to be send to client about our map
        if(cachedMapData == null) {

            MapGenData.Pack(out cachedMapData);

        }

        //Send over back to requested client
        TargetServerJoinReply(sender, cachedMapData);
    }

    [TargetRpc]
    protected void TargetServerJoinReply(NetworkConnectionToClient target, byte[] mapData) {

        //Receive the map from server, unpack here
        MapGenData.Unpack(mapData);

        clientReceivedMapData = true;
    }

    [Client]
    private IEnumerator ClientStartProcess() {

        clientReceivedMapData = false;

        yield return null;

        //We need the UI to blockkkk
        while(UIHUD.Instance == null) {

            yield return null;

        }

        //Show the loading
        UIHUD.Instance.ShowLoading(true);

        CmdClientJoinRequest();

        while (!clientReceivedMapData) {

            yield return null;

        }

        UIHUD.Instance.SetLoadingProgress(0.1f);

        yield return mapGenManager.GenerationProcess();

        UIHUD.Instance.SetLoadingProgress(1f);

        yield return null;

        UIHUD.Instance.ShowLoading(false);

        ClientReady = true;

    }

}