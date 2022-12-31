using UnityEngine;
using Mirror;

public abstract class ProjectileHitBase : NetworkBehaviour
{
    public ProjectileDamageSO damageSO;

    public void OnTriggerEnter(Collider other) {
        
        if(isServer){

            AttackHelper.Attack(gameObject, other.gameObject, damageSO);

            NetworkServer.Destroy(gameObject);

        }

    }

}