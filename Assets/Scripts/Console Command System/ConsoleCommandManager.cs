using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Reflection;
using System.Linq;
using System.Text;
using Unity.VisualScripting;

public static class ConsoleCommandManager
{
#if UNITY_EDITOR

    [MenuItem("ConsoleCommand/Generate Console Command")]
    private static void GenerateConsoleCommand() {

        Dictionary<string, Queue<string>> commandBuilderDic = new Dictionary<string, Queue<string>>();

        #region Load and find console command function

        var assembly = Assembly.GetExecutingAssembly();

        var methodsInfo = assembly.GetTypes().SelectMany(t => t.GetMethods()).Where(m => m.GetCustomAttribute(typeof(ConCommandAttribute)) != null).ToArray();

        foreach(var method in methodsInfo) {

            //Add the key (Class name)
            if (commandBuilderDic.ContainsKey(method.DeclaringType.Name) == false) {

                commandBuilderDic.Add(method.DeclaringType.Name, new Queue<string>());

                Debug.LogFormat("[Console Command Builder] Add New Key {0}", method.DeclaringType.Name);

            }

            //Add the method
            if (commandBuilderDic[method.DeclaringType.Name].Contains(method.Name) == false) {

                commandBuilderDic[method.DeclaringType.Name].Enqueue(method.Name);

                Debug.LogFormat("[Console Command Builder] Add New Function Value {0}", method.Name);

            }


        }

        #endregion

        #region Generation Class Data

        StringBuilder stringBuilder = new StringBuilder();

        foreach(var commandBuilder in commandBuilderDic) {

            //Start with class
            stringBuilder.AppendLine("public partial class" + commandBuilder.Key + " {");

            //Expand the function
            foreach (var command in commandBuilder.Value) {

                //Function name
                stringBuilder.AppendLine("\tpublic void CC" + command + " {");

                //Body
                stringBuilder.AppendLine("\t\t" + command + "();");

                //End
                stringBuilder.AppendLine("\t}");

            }

            //End
            stringBuilder.AppendLine("}");
        }

        UnityEngine.GUIUtility.systemCopyBuffer = stringBuilder.ToString();

        #endregion

    }

#endif
}

[AttributeUsage(AttributeTargets.Method)]
public class ConCommandAttribute : Attribute 
{
    /// <summary>
    /// Please make sure you're adding this in a partial class only
    /// </summary>
    /// <param name="commandName"></param>
    public ConCommandAttribute(string commandName)
    {


    }
}