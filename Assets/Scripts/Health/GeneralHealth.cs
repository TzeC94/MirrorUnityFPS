using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralHealth : BaseDeath, IHitable {

    public virtual void OnHit(HitInfo hitInfo) {

        if (isServer) {

            currentHealth -= hitInfo.damage;

            if (DeathCheck()) {

                OnKilled(hitInfo);

            }

        }

    }
}
