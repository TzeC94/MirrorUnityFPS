using UnityEngine;

public static class AttackHelper
{
    public static void Attack(GameObject attacker, GameObject target, ProjectileDamageSO damageSO) {

        var iHitable = target.GetComponent<IHitable>();

        if (iHitable != null) {

            HitInfo hitInfo = new HitInfo();
            hitInfo.damage = damageSO.damage;
            hitInfo.attackerPos = attacker.transform.position;
            iHitable.OnHit(hitInfo);

        }

    }
}