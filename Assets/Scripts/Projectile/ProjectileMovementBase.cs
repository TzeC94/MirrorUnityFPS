using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

[RequireComponent(typeof(Rigidbody))]
public abstract class ProjectileMovementBase : NetworkBehaviour
{
    public enum MoveDirection { Forward, Backward, Left, Right, Up, Down }

    //Component
    protected Rigidbody rigidBody;

    [Header("Movement")]
    public float moveSpeed = 1f;
    public MoveDirection moveDirection = MoveDirection.Forward;
    private Vector3 movDirection;   //This will assign on runtime

    public virtual void Awake() {

        rigidBody = GetComponent<Rigidbody>();

    }

    public virtual void Start() {

        switch (moveDirection) {
            case MoveDirection.Forward:
                movDirection = transform.forward;
                break;
            case MoveDirection.Backward:
                movDirection = -transform.forward;
                break;
            case MoveDirection.Up:
                movDirection = transform.up;
                break;
            case MoveDirection.Down:
                movDirection = -transform.up;
                break;
            case MoveDirection.Left:
                movDirection = -transform.right;
                break;
            case MoveDirection.Right:
                movDirection = transform.right;
                break;
        }

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

        AddForce(movDirection);

    }

    public virtual void AddForce(Vector3 direction){

        //Add the rigidbody movement
        rigidBody.AddForce(direction * moveSpeed * Time.fixedDeltaTime);

    }

    #endregion

}