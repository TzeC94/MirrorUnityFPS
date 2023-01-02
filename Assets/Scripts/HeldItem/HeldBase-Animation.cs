using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public abstract partial class HeldBase
{
    [Header("Animation")]
    public Animator animator;

    private int anim_Fire = Animator.StringToHash("Fire");
    private int anim_Reload = Animator.StringToHash("Reload");

    public void AnimFire() {

        animator.SetTrigger(anim_Fire);

    }

    public void AnimReload() {

        animator.SetTrigger(anim_Reload);

    }
}
