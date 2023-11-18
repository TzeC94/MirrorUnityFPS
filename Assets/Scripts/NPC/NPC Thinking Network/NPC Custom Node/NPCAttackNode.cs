using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Attack Node", menuName = "AI/Attack Node")]
public class NPCAttackNode : NPCThinkNode
{
    public override int outputNodeCount => 2;

    private GameObject target;

    public float mininumDistanceAttack = 2f;
    public float maxDistanceToAbordAttack = 3f; //Something when target moved too far

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

        myThinkTree.currentNPC.NPC_AttackStart();

    }

    public override void OnUpdate() {

        AttackUpdate();

    }

    protected override void OnFailed() {

        OnEnd(1);

    }

    protected virtual bool MeetAttackCondition(float distance) {

        return distance < mininumDistanceAttack;

    }

    public virtual bool TooFarFromAttack(float distance) {

        return distance > maxDistanceToAbordAttack;

    }

    protected virtual void AttackUpdate() {

        var distance = Vector3.Distance(myThinkTree.currentNPC.transform.position, target.transform.position);

        if (MeetAttackCondition(distance)) {

            myThinkTree.currentNPC.NPC_AttackUpdate();

        }

        //IF target too far
        if(TooFarFromAttack(distance)) {

            OnFailed();

        }

    }

}