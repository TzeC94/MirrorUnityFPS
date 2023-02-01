using Mirror;
using UnityEngine;
using System;

public class BaseCombatScript : BaseScriptNetwork, IHitable {

    [Header("Health")]
    [SerializeField]
    private int maxHealth = 100;
    [SyncVar]
    protected int currentHealth = 0;
    public int CurrentHealth { get { return currentHealth; } }
    public bool IsDead { get { return currentHealth <= 0f; } }

    //Death
    public Action OnDeathCallback;

    // Start is called before the first frame update
    public override void Start() {

        base.Start();

        Initialize();

    }

    public virtual void Initialize() {

        ResetHealth();

    }

    #region Health

    public virtual void ResetHealth() {

        if (isServer) {
            currentHealth = maxHealth;
        }

    }

    #endregion

    #region Death

    public virtual bool DeathCheck() {

        return IsDead;

    }

    public virtual void OnKilled(HitInfo hitInfo) {

        OnDeathCallback?.Invoke();
        RPCOnKilled(hitInfo);

    }

    public virtual void Respawn() {

        ResetHealth();

    }

    [ClientRpc]
    public virtual void RPCOnKilled(HitInfo hitInfo) {



    }

    #endregion

    [Server]
    public virtual void OnHit(HitInfo hitInfo) {

        if (isServer) {

            currentHealth -= hitInfo.damage;
            RPC_ClientOnHit(hitInfo);

            if (DeathCheck()) {

                OnKilled(hitInfo);

            }

        }

    }

    [ClientRpc]
    public void RPC_ClientOnHit(HitInfo hitInfo) {

        WorldPopup.instance.SpawnPopup(hitInfo.damage.ToString(), hitInfo.hitPoint);

    }
}
