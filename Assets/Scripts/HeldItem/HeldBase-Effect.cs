using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract partial class HeldBase
{
    public enum EffectType { Fire }

    [Header("Effect")]
    public EffectPlayer fireEffect;

    /// <summary>
    /// To send RPC to client to play this effect
    /// </summary>
    [Server]
    public void ServerPlayerEffect(EffectType effectType) {

        PlayerEffect(effectType);

    }

    [ClientRpc]
    private void PlayerEffect(EffectType effectType) {

        EffectPlayer targetEffect;

        switch (effectType) {
            case EffectType.Fire:
                targetEffect = fireEffect;
                break;
            default:
                targetEffect = null;
                break;
        }

        if(targetEffect != null) {

            targetEffect.PlayerEffect();

        }

    }
}
