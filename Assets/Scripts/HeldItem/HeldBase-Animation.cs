using Mirror;
using UnityEngine;

/*
 * Call this on server
 * Let Mirror Network Animator to sync the animation event from Server to Client
 */

public abstract partial class HeldBase
{
    [Header("Animation")]
    public NetworkAnimator animator;

    private static int anim_Fire = Animator.StringToHash("Fire");
    private static int anim_Reload = Animator.StringToHash("Reload");

    public void AnimFire() {

        animator.SetTrigger(anim_Fire);

    }

    public void AnimReload() {

        animator.SetTrigger(anim_Reload);

    }
}
