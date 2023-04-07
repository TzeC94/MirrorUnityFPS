using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract partial class BaseNPC
{
    [Header("AI")]
    [SerializeField]
    private NavMeshAgent navAgent;
    public NavMeshAgent NavAgent { get { return navAgent; } }
    public float RemainingDistance { get { return navAgent.remainingDistance; } }
    public bool CloseToTarget { get { return navAgent.remainingDistance <= navAgent.stoppingDistance; } }

    public void MoveTo(Vector3 targetPos) {

        if(navAgent != null) {

            navAgent.SetDestination(targetPos);

        }

    }

}