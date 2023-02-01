using UnityEngine;

public class HeldLocal : MonoBehaviour, IHeld
{
    [Header("Animation")]
    public Animator animator;

    public void AnimFire() {

        animator.SetTrigger(HeldBase.anim_Fire);
    }

    public void AnimReload() {

        animator.SetTrigger(HeldBase.anim_Reload);

    }

    [Header("Effect")]
    public EffectPlayer fireEffect;

    public void PlayerEffect(HeldBase.EffectType effectType) {

        if (fireEffect) {
            fireEffect.PlayerEffect();
        }

    }

}