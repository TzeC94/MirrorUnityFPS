using System.Collections;
using UnityEngine;
using Mirror;

public class CharacterAnimator : BaseAnimator
{
    [Header("3rd Person Model")]
    [SerializeField]
    private GameObject playerModel;

    //Key
    private static int verticalKey = Animator.StringToHash("Vertical");
    private static int horizontalKey = Animator.StringToHash("Horizontal");
    private static int onGround = Animator.StringToHash("OnGround");

    NetworkBehaviour networkBehaviour;

    // Start is called before the first frame update
    protected new IEnumerator Start() {

        base.Start();

        networkBehaviour = animatorInterface.MyNetworkBehaviour();

        if (networkBehaviour.isClient && playerModel != null) {

            while (!NetworkClient.isConnected) {
                yield return null;
            }

            InitializeModel(networkBehaviour.isLocalPlayer);

        }
    }

    private void InitializeModel(bool isLocalPlayer) {

        if (playerModel != null) {
            //We dont want this to be enable if is local player
            playerModel.SetActive(!isLocalPlayer);
        }

    }

    protected override void FixedUpdate() {

        if (networkBehaviour.isServer) {

            if (networkAnimator != null) {

                var movingVelocity = animatorInterface.Velocity();

                networkAnimator.animator.SetFloat(verticalKey, movingVelocity.y);
                networkAnimator.animator.SetFloat(horizontalKey, movingVelocity.x);

                networkAnimator.animator.SetBool(onGround, animatorInterface.OnGround());

            }

        }

    }
}
