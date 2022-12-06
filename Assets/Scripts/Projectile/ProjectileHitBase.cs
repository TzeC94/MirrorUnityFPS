using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public abstract class ProjectileHitBase : NetworkBehaviour
{
    public int damage = 10;

    // Start is called before the first frame update
    public virtual void Start()
    {
        
    }

    // Update is called once per frame
    public virtual void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other) {
        
        if(isServer){

            var iHitable = other.gameObject.GetComponent<IHitable>();

            if (iHitable != null) {

                HitInfo hitInfo = new HitInfo();
                hitInfo.damage = damage;
                hitInfo.attackerPos = transform.position;
                iHitable.OnHit(hitInfo);

            }

            NetworkServer.Destroy(gameObject);

        }

    }

}
