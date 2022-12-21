using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerHeld : NetworkBehaviour
{
    private HeldBase _currentHeld;
    public HeldBase currentHeld { get { return _currentHeld; } }

    // Start is called before the first frame update
    void Start()
    {

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

    public void SetCurrentHeld(HeldBase heldObject) {

        _currentHeld = heldObject;

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
            _currentHeld.Fire();

            s_FirePressed = false;

        }

    }

    #endregion

}