using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract partial class BaseNPC
{
    [Header("AI")]
    [SerializeField]
    private NavMeshAgent navAgent;

    public void MoveTo(Vector3 targetPos) {

        if(navAgent != null) {

            navAgent.SetDestination(targetPos);

        }

    }

    public bool CloseToTarget() {

        return navAgent.remainingDistance <= navAgent.stoppingDistance;

    }

}
