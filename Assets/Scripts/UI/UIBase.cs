using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public abstract class UIBase : MonoBehaviour
{
    public bool isOpen { internal set; get; }

    public virtual void Init(){

        isOpen = true;

    }

    public virtual void Open(){

        if(isOpen)
            return;

        isOpen = true;
        gameObject.SetActive(isOpen);
    }

    public virtual void Close(){

        if(!isOpen)
            return;

        isOpen = false;
        gameObject.SetActive(isOpen);

    }

}
