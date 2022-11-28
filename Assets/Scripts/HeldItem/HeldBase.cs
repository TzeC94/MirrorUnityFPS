using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public abstract class HeldBase : ItemBase
{
    [HideInInspector] public GameObject ownerObject;

    public virtual void Fire(){
    }

}