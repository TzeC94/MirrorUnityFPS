using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

[RequireComponent(typeof(Rigidbody))]
public abstract class ProjectileMovementBase : NetworkBehaviour
{
    public enum MoveDirection { Forward, Backward, Left, Right, Up, Down }

    //Component
    private Rigidbody rigidBody;

    [Header("Movement")]
    public float moveSpeed = 1f;
    public MoveDirection moveDirection = MoveDirection.Forward;

    public virtual void Awake() {

        rigidBody = GetComponent<Rigidbody>();

    }

    // Start is called before the first frame update
    public virtual void Start()
    {
        
    }

    // Update is called once per frame
    public virtual void Update()
    {
        
    }

    public virtual void FixedUpdate() {

        if(isServer){

            MoveProjectile();

        }

    }

    #region Movement

    /// <summary>
    /// Update the position of this projectile
    /// </summary>
    public virtual void MoveProjectile(){

        Vector3 v_MoveDirection = Vector3.zero;

        switch(moveDirection){
            case MoveDirection.Forward:
                v_MoveDirection = transform.forward;
                break;
            case MoveDirection.Backward:
                v_MoveDirection = transform.forward;
                break;
            case MoveDirection.Up:
                v_MoveDirection = transform.forward;
                break;
            case MoveDirection.Down:
                v_MoveDirection = transform.forward;
                break;
            case MoveDirection.Left:
                v_MoveDirection = transform.forward;
                break;
            case MoveDirection.Right:
                v_MoveDirection = transform.forward;
                break;
        }

        //Add the rigidbody movement
        rigidBody.AddForce(v_MoveDirection * moveSpeed * Time.fixedDeltaTime);

    }

    #endregion

}