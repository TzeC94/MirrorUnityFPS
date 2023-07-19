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

    protected override void Start() {

        base.Start();

        if (isServer) {

            StartCoroutine(ServerStartProcess());

        }

        if (isClient) {

            CmdClientJoinRequest();

        }

    }

    private IEnumerator ServerStartProcess() {

        yield return mapGenManager.GenerationProcess();

        yield return null;

    }

    [Command]
    protected void CmdClientJoinRequest(NetworkConnectionToClient sender = null) {

        //Gather the byte data that need to be send to client about our map
        if(cachedMapData == null) {

            MapGenData.Pack(out cachedMapData);

        }
        
        //Send over back to requested client
        RPCServerJoinReply(sender, cachedMapData);
    }

    [TargetRpc]
    protected void RPCServerJoinReply(NetworkConnectionToClient target, byte[] mapData) {

        //Receive the map from server, unpack here
        MapGenData.Unpack(ref mapData);

        //TODO: Then we should start the map generation on client side base on received data
        StartCoroutine(ClientStartProcess());

    }

    private IEnumerator ClientStartProcess() {

        yield return null;

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