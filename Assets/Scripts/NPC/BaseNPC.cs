using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract partial class BaseNPC : PlayerBase
{
    public override void OnStartClient() {

        base.OnStartClient();

        //Disable on client
        navAgent.enabled = false;

    }

    public override void OnStartServer() {
        
        base.OnStartServer();

        InitializeThinkTree();

    }

}