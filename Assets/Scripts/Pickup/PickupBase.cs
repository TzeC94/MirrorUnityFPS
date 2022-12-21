using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupBase : BaseScript
{
    public ItemData itemData;
    public bool canPickUp = true;

    public virtual void Pickup(uint callerPlayerID) {

        GameManagerBase.instance.PickupItemToInventory(netId, callerPlayerID);

    }

    public override ActionCollection[] GetActions(uint callerPlayerID) {

        ActionCollection[] action = new ActionCollection[1];

        var action0 = action[0];

        action0.actionTitle = "Pickup";
        action0.action = () => Pickup(callerPlayerID);
        action[0] = action0;

        return action;

    }

}
