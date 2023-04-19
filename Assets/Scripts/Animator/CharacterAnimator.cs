using UnityEngine;

public class CharacterAnimator : BaseAnimator
{
    [Header("3rd Person Model")]
    [SerializeField]
    private GameObject characterModel;

    //Key
    private static int verticalKey = Animator.StringToHash("Vertical");
    private static int horizontalKey = Animator.StringToHash("Horizontal");
    private static int onGround = Animator.StringToHash("OnGround");

    // Start is called before the first frame update
    protected override void Start() {

        base.Start();

        InitializeModel(animatorInterface.MyNetworkBehaviour().isLocalPlayer);

    }

    private void InitializeModel(bool isLocalPlayer) {

        if (characterModel != null) {
            //We dont want this to be enable if is local player
            characterModel.SetActive(isLocalPlayer);
        }

    }

    protected override void FixedUpdate() {

        if (animatorInterface.MyNetworkBehaviour().isServer) {

            if (networkAnimator != null) {

                var movingVelocity = animatorInterface.Velocity();

                networkAnimator.animator.SetFloat(verticalKey, movingVelocity.y);
                networkAnimator.animator.SetFloat(horizontalKey, movingVelocity.x);

                networkAnimator.animator.SetBool(onGround, animatorInterface.OnGround());

            }

        }

    }
}
