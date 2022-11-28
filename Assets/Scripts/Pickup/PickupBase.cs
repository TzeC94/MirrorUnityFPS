using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupBase : BaseScript
{
    public ItemData itemData;
    public bool canPickUp = true;

    public override void ActionOne(uint callerPlayerID) {

        if (canPickUp) {

            Pickup(callerPlayerID);

        }
        
    }

    public virtual void Pickup(uint callerPlayerID) {

        GameManagerBase.instance.PickupItemToInventory(netId, callerPlayerID);

    }

}
