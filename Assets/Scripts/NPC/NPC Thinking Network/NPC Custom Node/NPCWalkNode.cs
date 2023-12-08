using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "Walk Node", menuName = "AI/Walk Node")]
public class NPCWalkNode : NPCThinkNode {

    private GameObject target;

    public float stoppingDistance = 3f;
    public float stopChaseDistance = 3f;


    //Navigation
    private NavMeshAgent navAgent;
    private NavMeshPath navMeshPath;
    private const int updateFrequency = 5;  //Update to target every x count
    private int currentUpdateCount = 0;

    public override int outputNodeCount => 2;

#if UNITY_EDITOR

    protected override void OnValidate() {

        base.OnValidate();

        titleNameList[0] = "Reach Target";
        titleNameList[1] = "Reach Failed";

    }

#endif

    protected override void OnFailed() {

        //Stop the agent
        navAgent.isStopped = true;

        EndNode(1);

    }

    protected override void OnStart() {

        navAgent = myThinkTree.currentNPC.NavAgent;

        if (navMeshPath == null)
            navMeshPath = new NavMeshPath();

        //Grab my target
        target = myThinkTree.unityTypeSharedData[NPCHelper.targetString] as GameObject;

        //Generate the path
        UpdatePath();

    }

    protected override void OnUpdate() {

        //Update to target
        if (currentUpdateCount >= updateFrequency) {

            currentUpdateCount = 0;

            //Generate the path
            if (!UpdatePath())
                return;

        } else {

            currentUpdateCount++;

        }

        if (myThinkTree.currentNPC.RemainingDistance <= stoppingDistance) {

            EndNode(0);

        }

    }

    private bool UpdatePath() {

        if (target != null) {

            //Generate the path
            navAgent.CalculatePath(target.transform.position, navMeshPath);

            if (navMeshPath.status == NavMeshPathStatus.PathComplete) {

                bool successApplied = navAgent.SetPath(navMeshPath);

                //Check is too far to chase
                if (successApplied && navAgent.remainingDistance < stopChaseDistance) {

                    if (navAgent.isStopped)
                        navAgent.isStopped = false;

                    return true;

                }
            }

        }

        OnFailed();
        return false;
    }

    protected override void OnEnd() {
        
    }
}