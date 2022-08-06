using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public abstract class HeldBase : MonoBehaviour
{
    [HideInInspector] public GameObject ownerObject;

    // Start is called before the first frame update
    public virtual void Start(){
        
    }

    // Update is called once per frame
    public virtual void Update(){
        
    }

    public virtual void Fire(){
    }

}