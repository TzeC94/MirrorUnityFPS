using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerBase
{
    [Header("3rd Person Model")]
    [SerializeField]
    private GameObject characterModel;
    [SerializeField]
    private NetworkAnimator networkAnimator;

    //Key
    private static int verticalKey = Animator.StringToHash("Vertical");
    private static int horizontalKey = Animator.StringToHash("Horizontal");
    private static int onGround = Animator.StringToHash("OnGround");

    //Value
    private float vertical, horizontal;
    private bool onGrounded;

    private void InitializeModel() {

        if(characterModel != null) {
            //We dont want this to be enable if is local player
            characterModel.SetActive(!isLocalPlayer);
        }

    }

    public virtual void TPS_AnimatorUpdate() {

        networkAnimator.animator.SetFloat(verticalKey, vertical);
        networkAnimator.animator.SetFloat(horizontalKey, horizontal);

        networkAnimator.animator.SetBool(onGround, onGrounded);

    }

    /// <summary>
    /// To set the value for the animator
    /// </summary>
    /// <param name="vertical"></param>
    /// <param name="horizontal"></param>
    /// <param name="onGrounded"></param>
    public void UpdateValue(float vertical, float horizontal, bool onGrounded) {

        this.vertical = vertical;
        this.horizontal = horizontal;
        this.onGrounded= onGrounded;

    }

}
