using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerProcBase : GameManagerBase
{
    [Header("Map Generation")]
    public MapGenManager mapGenManager;

    public static GameManagerProcBaseData procBaseData;

    protected override void Start() {

        base.Start();

        if(procBaseData == null) {

            procBaseData = new GameManagerProcBaseData();

        }

        if(isServer) {

            StartCoroutine(StartLoopServer());

        }

        if(isClient) {

            //Client to ask Server to send related file
            CmdStartRequest();

        }

    }

    private IEnumerator StartLoopServer() {

        yield return mapGenManager.GenerationProcess();

        yield return null;

    }

    [Command]
    protected void CmdStartRequest() {

        //Send whatever needed from server to client


    }
}