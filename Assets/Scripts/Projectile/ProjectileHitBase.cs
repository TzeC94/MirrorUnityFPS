using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public abstract class ProjectileHitBase : NetworkBehaviour
{
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

            NetworkServer.Destroy(gameObject);

        }

    }

}
