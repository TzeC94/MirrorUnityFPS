using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerProcBase : GameManagerBase
{
    [Header("Map Generation")]
    public MapGenManager mapGenManager;

    //Cached map data
    byte[] cachedMapData;

    protected override void Start() {

        base.Start();

        if(isServer) {

            StartCoroutine(ServerStartLoopServer());

        }

        if(isClient) {

            //Client to ask Server to send related file
            CmdClientJoinRequest();

        }

    }

    private IEnumerator ServerStartLoopServer() {

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


    }
}