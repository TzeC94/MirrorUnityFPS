using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class bootstrap : MonoBehaviour
{
    private const string trueString = "true";
    private const string falseString = "false";

    // Start is called before the first frame update
    void Start()
    {
        //Read the Args
        string[] envArgs = System.Environment.GetCommandLineArgs();

        for(int i = 0; i < envArgs.Length; i++) {

            var envToCheck = envArgs[i];

            if (envToCheck.StartsWith("-")) {

                if (envToCheck == "-ServerBuild") {

                    if (envArgs[i+1] == trueString)
                        GlobalVar.isServerBuild = true;

                }

            }

        }

        //Load to Main Menu
        SceneManager.LoadScene("Main Menu");
        
    }
}
