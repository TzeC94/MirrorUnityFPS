using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterUIScript : MonoBehaviour
{
    public static MasterUIScript instance;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null) {
            instance = this;
        }
    }

    private void OnDestroy() {

        instance = null;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
