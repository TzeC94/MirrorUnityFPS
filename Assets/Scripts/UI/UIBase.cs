using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIBase : MonoBehaviour
{
    public virtual void Init(){
    }

    public virtual void Open(){

        gameObject.SetActive(true);

    }

    public virtual void Close(){

        gameObject.SetActive(false);

    }

}
