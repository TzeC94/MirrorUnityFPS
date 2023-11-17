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

    //---Animation--//

    public static int anim_Fire = Animator.StringToHash("Fire");
    public static int anim_Reload = Animator.StringToHash("Reload");

    public void AnimFire() {

        if (animator != null) {

            animator.SetTrigger(anim_Fire);

        }
        
        if(ownerObject.playerAnimator != null) {

            ownerObject.playerAnimator.PlayAttack();

        }

    }

    public void AnimReload() {

        if(animator != null) {
            animator.SetTrigger(anim_Reload);
        }
        
        if(ownerObject.playerAnimator != null) {
            ownerObject.playerAnimator.PlayReload();
        }

    }
}
