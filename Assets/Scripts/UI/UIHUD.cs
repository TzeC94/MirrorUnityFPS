using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class UIHUD : UIBase
{
    public static UIHUD Instance { get { return instance; } }
    private static UIHUD instance;

    private void Start() {
        
        if(instance == null) {
            instance = this;
        }

    }
}