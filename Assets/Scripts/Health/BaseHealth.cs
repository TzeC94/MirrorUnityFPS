using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class BaseHealth : NetworkBehaviour
{
    [SerializeField]
    private int maxHealth = 100;
    [SyncVar]
    protected int currentHealth = 0;
    public int CurrentHealth { get { return currentHealth; } }

    // Start is called before the first frame update
    public virtual void Start()
    {
        Initialize();
    }

    public virtual void Initialize(){

        ResetHealth();

    }

    public virtual void ResetHealth() {

        if(isServer){
            currentHealth = maxHealth;
        }
       
    }

}