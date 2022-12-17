using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class bootstrap : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //Read the Args
        string[] envArgs = System.Environment.GetCommandLineArgs();

        for(int i = 0; i < envArgs.Length; i++) {

            Debug.Log($"Processing {envArgs[i]}");

            if (envArgs[i] == "-ServerBuild") {

                GlobalVar.isServerBuild = true;

            }

        }

        //Load to Main Menu
        SceneManager.LoadScene("Main Menu");
        
    }
}
