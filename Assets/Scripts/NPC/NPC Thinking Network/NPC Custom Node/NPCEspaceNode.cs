using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "Espace Node", menuName = "AI/Espace Node")]
public class NPCEspaceNode : NPCThinkNode
{
    public override int outputNodeCount => 1;

    private NavMeshAgent myNavAgent;

    [Header("Espace")]
    [SerializeField] private float espaceRange = 10f;   //How far to espace

#if UNITY_EDITOR

    protected override void Reset() {

        if (titleNameList.Length != outputNodeCount) {

            titleNameList = new string[outputNodeCount];

        }

        titleNameList[0] = "Espace Done";

    }

#endif

    protected override void OnEnd() {
        
    }

    protected override void OnFailed() {
        
    }

    protected override void OnStart() {

        myNavAgent = myThinkTree.currentNPC.NavAgent;

        //Look for espace point
        bool found = false;
        Vector3 myPos = myThinkTree.currentNPC.transform.position;

        while (found == false) {

            //Random Direction
            Vector3 randomDirection = Random.insideUnitCircle * espaceRange;

            const float SAMPLE_RANGE = 1.0f;
            if (NavMesh.SamplePosition(myPos + randomDirection, out NavMeshHit hit, SAMPLE_RANGE, NavMesh.AllAreas)) {

                //Navigate to that point
                myNavAgent.SetDestination(hit.position);

                found = true;

            }
        }

    }

    protected override void OnUpdate() {

        //Always check we are close
        if(myNavAgent.remainingDistance <= myNavAgent.stoppingDistance) {

            EndNode(0);

        }

    }
}