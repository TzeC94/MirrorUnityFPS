using UnityEngine;
using Mirror;

public abstract class ProjectileHitBase : NetworkBehaviour
{
    public ProjectileDamageSO damageSO;

    public void OnTriggerEnter(Collider other) {
        
        if(isServer){
            
            //Decal Spawner
            {
                var direction = (other.transform.position - transform.position).normalized;
                DecalSpawner.instance.SpawnDecal(other, other.ClosestPoint(transform.position), Quaternion.LookRotation(direction));
            }
            
            AttackHelper.Attack(gameObject, other.gameObject, damageSO);

            NetworkServer.Destroy(gameObject);

        }

    }

}