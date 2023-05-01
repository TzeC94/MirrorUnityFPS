using Mirror;
using UnityEngine;

public abstract class BaseAnimator : MonoBehaviour
{
    [Header("Controller")]
    public Animator animator;
    public NetworkAnimator networkAnimator;

    protected IAnimator animatorInterface;

    protected virtual void Start() {

        animatorInterface = GetComponent<IAnimator>();

    }

    protected virtual void OnEnabled() {

    }

    protected virtual void OnDisabed() {

    }

    protected virtual void FixedUpdate() {

    }

}
