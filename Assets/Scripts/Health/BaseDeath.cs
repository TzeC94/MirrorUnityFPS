using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseDeath : BaseHealth
{
    public Action OnDeathCallback;

    public virtual bool DeathCheck() {

        return IsDead;

    }

    public virtual void OnKilled(HitInfo hitInfo) {

        OnDeathCallback?.Invoke();
        RPCOnKilled(hitInfo);

    }

    public virtual void Respawn() {

        ResetHealth();

    }

    [ClientRpc]
    public virtual void RPCOnKilled(HitInfo hitInfo) {



    }

}
