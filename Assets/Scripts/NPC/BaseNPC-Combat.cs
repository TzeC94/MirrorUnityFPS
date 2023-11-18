public partial class BaseNPC
{
    public virtual void NPC_AttackStart() {

    }

    public virtual void NPC_AttackUpdate() {

        if (playerHeld == null)
            return;

        var myHeld = playerHeld.currentHeld;

        if (myHeld == null)
            return;

        //Range
        if(myHeld is HeldRange heldRange) {

            if (heldRange.enoughBullet) {

                heldRange.Fire();

            } else {    //Try to reload

                heldRange.ServerReload();

            }

        }

        //No melee yet
        if(myHeld is HeldMelee heldMelee) {

            heldMelee.Fire();

        }

    }
}