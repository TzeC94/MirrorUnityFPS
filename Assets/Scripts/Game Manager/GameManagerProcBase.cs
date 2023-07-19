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

            Debug.Log("Call");
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

        Debug.Log($"Server Receive Client Join Request {cachedMapData.Length}");

        //Send over back to requested client
        TargetServerJoinReply(sender, cachedMapData);
    }

    [TargetRpc]
    protected void TargetServerJoinReply(NetworkConnectionToClient target, byte[] mapData) {

        Debug.Log($"Client Receive Client Join Request {mapData.Length}");

        //Receive the map from server, unpack here
        MapGenData.Unpack(mapData);

        clientReceivedMapData = true;
    }

    [Client]
    private IEnumerator ClientStartProcess() {

        clientReceivedMapData = false;

        yield return null;

        CmdClientJoinRequest();

        while (!clientReceivedMapData) {

            yield return null;

        }

        yield return mapGenManager.GenerationProcess();

        yield return null;

        //Tell the server I'm ready
        CmdClientReady();
    }

    [Command]
    protected void CmdClientReady(NetworkConnectionToClient sender = null) {

        /*
         * Server receive this when client finish all the map generation
         */

        //Spawn player for this
        MyNetworkManager.singleton.OnServerAddPlayer(sender);

    }

}