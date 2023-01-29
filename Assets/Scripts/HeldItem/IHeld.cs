using Mirror;
using UnityEngine;

public interface IHeld
{
    public void PlayerEffect(HeldBase.EffectType effectType);

    public void AnimFire();

    public void AnimReload();
}