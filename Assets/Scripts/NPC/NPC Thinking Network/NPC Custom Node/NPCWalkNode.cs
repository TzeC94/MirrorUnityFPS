using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "Walk Node", menuName = "AI/Walk Node")]
public class NPCWalkNode : NPCThinkNode {

    private GameObject target;

    public float stoppingDistance = 3f;


    //Navigation
    private NavMeshAgent navAgent;
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

        OnEnd(1);

    }

    public override void OnStart() {

        navAgent = myThinkTree.currentNPC.NavAgent;

        //Grab my target
        target = myThinkTree.unityTypeSharedData[NPCHelper.targetString] as GameObject;

        //Generate the path
        UpdatePath();

    }

    public override void OnUpdate() {

        //Update to target
        if(currentUpdateCount >= updateFrequency) {

            currentUpdateCount = 0;

            //Generate the path
            if (!UpdatePath())
                return;

        } else {

            currentUpdateCount++;

        }

        if (myThinkTree.currentNPC.RemainingDistance <= stoppingDistance) {

            OnEnd(0);

        }

    }

    private bool UpdatePath() {

        //Generate the path
        NavMeshPath navMeshPath = null;
        navAgent.CalculatePath(target.transform.position, navMeshPath);

        if (navMeshPath.status == NavMeshPathStatus.PathComplete) {

            navAgent.SetPath(navMeshPath);
            return true;

        }

        OnFailed();
        return false;
    }
}