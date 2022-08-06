using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerHeld : NetworkBehaviour
{

    public HeldBase currentHeld;

    // Start is called before the first frame update
    void Start()
    {
        if(currentHeld != null){

            currentHeld.ownerObject = gameObject;

        }
    }

    // Update is called once per frame
    void Update()
    {
        if(isClient){

            Client_FirePressed();

        }

        if(isServer){

            Server_FirePressed();

        }
    }

    #region Fire Button

    void Client_FirePressed(){

        if(PlayerInputInstance.instance.fire){

            CmdFirePressed();

            PlayerInputInstance.instance.fire = false;
        }

    }

    bool s_FirePressed = false;

    [Command]
    void CmdFirePressed(){

        s_FirePressed = true;

    }

    void Server_FirePressed(){

        if(s_FirePressed){

            //Do Something
            currentHeld.Fire();

            s_FirePressed = false;

        }

    }

    #endregion

}