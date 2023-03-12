using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Attack Node", menuName = "AI/Attack Node")]
public class NPCAttackNode : NPCThinkNode
{
    public override int outputNodeCount => 2;

    private GameObject target;

    public float mininumDistanceAttack = 2f;

#if UNITY_EDITOR

    protected override void OnValidate() {

        base.OnValidate();

        titleNameList[0] = "Kill Target";
        titleNameList[1] = "Fail To Kill";

    }

#endif

    public override void OnStart() {

        //Grab my target
        target = myThinkTree.unityTypeSharedData[NPCHelper.targetString] as GameObject;

    }

    public override void OnUpdate() {
        throw new System.NotImplementedException();
    }

    protected override void OnFailed() {
        throw new System.NotImplementedException();
    }

    protected virtual bool MeetAttackCondition() {

        //Distance to player
        var distance = Vector3.Distance(myThinkTree.currentNPC.transform.position, target.transform.position);

        return distance < mininumDistanceAttack;

    }

    protected virtual void AttackUpdate() {

        if (MeetAttackCondition()) {



        }

    }

}