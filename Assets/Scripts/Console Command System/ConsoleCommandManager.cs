using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Reflection;
using System.Linq;
using System.Text;
using System.Data.Common;

public static partial class ConsoleCommandManager
{
#if UNITY_EDITOR

    private const string SAVE_FOLDER = "/Scripts/Console Command System/Generated";
    private const string FILE_NAME = "/ConsoleCommandManager-Generated.cs";

    private static string Save_Path { get { return Application.dataPath + SAVE_FOLDER + FILE_NAME; } }

    [MenuItem("ConsoleCommand/Generate Console Command")]
    private static void GenerateConsoleCommand() {

        Dictionary<string, CommandQueue> commandBuilderDic = new Dictionary<string, CommandQueue>();

        #region Load and find console command function

        var assembly = Assembly.GetExecutingAssembly();

        var methodsInfo = assembly.GetTypes().SelectMany(t => t.GetMethods()).Where(m => m.GetCustomAttribute(typeof(ConCommandAttribute)) != null).ToArray();

        foreach(var method in methodsInfo) {

            //Add the key (Class name)
            if (commandBuilderDic.ContainsKey(method.DeclaringType.Name) == false) {

                commandBuilderDic.Add(method.DeclaringType.Name, new CommandQueue());

                Debug.LogFormat("[Console Command Builder] Add New Key {0}", method.DeclaringType.Name);

            }

            //Add the method
            if (commandBuilderDic[method.DeclaringType.Name].ContainMethod(method.Name) == false) {

                ConCommandAttribute commandAttribute = method.GetCustomAttribute(typeof(ConCommandAttribute)) as ConCommandAttribute;

                commandBuilderDic[method.DeclaringType.Name].Add(commandAttribute.commandName, method.Name);

                Debug.LogFormat("[Console Command Builder] Add New Function Value {0}", method.Name);

            }


        }

        #endregion

        #region Generation Class Data

        StringBuilder stringBuilder = new StringBuilder();

        stringBuilder.AppendLine("using System;");

        //Start with class
        stringBuilder.AppendLine("public static partial class ConsoleCommandManager {");

        stringBuilder.AppendLine("");

        stringBuilder.AppendLine("\tpublic struct Command{");
        stringBuilder.AppendLine("\t\tpublic string name;");
        stringBuilder.AppendLine("\t\tpublic Action callAction;");
        stringBuilder.AppendLine("\t}");

        stringBuilder.AppendLine("");

        stringBuilder.AppendLine("\tpublic static Command[] command = {");

        foreach (var commandBuilder in commandBuilderDic) {

            //Expand the function
            foreach (var command in commandBuilder.Value) {

                stringBuilder.AppendLine("\t\tnew Command{");

                //Function name
                stringBuilder.AppendLine("\t\t\tname = \u0022" + command.commandName + "\u0022,");

                //Body
                stringBuilder.AppendLine("\t\t\tcallAction = " + commandBuilder.Key + "." + command.methodName);

                //End
                stringBuilder.AppendLine("\t\t}");
            }

        }

        stringBuilder.AppendLine("\t};");

        //End
        stringBuilder.AppendLine("}");

        UnityEngine.GUIUtility.systemCopyBuffer = stringBuilder.ToString();

        #endregion

        #region Saving

        //Check existing file and delete it
        if (File.Exists(Save_Path)) {
            File.Delete(Save_Path);
        }

        Debug.Log("Save generated code in folder " + Application.dataPath + SAVE_FOLDER);

        File.WriteAllText(Save_Path, stringBuilder.ToString());

        AssetDatabase.Refresh();

        #endregion

    }

#endif
}

[AttributeUsage(AttributeTargets.Method)]
public class ConCommandAttribute : Attribute 
{
    public string commandName;

    /// <summary>
    /// Please make sure you're adding this in a partial class only
    /// </summary>
    /// <param name="commandName"></param>
    public ConCommandAttribute(string commandName)
    {
        this.commandName = commandName;
    }
}

public struct CommandData {

    public string commandName;
    public string methodName;

}

public class CommandQueue : Queue<CommandData> {

    public void Add(string commandName, string methodName) {

        CommandData commandData = new CommandData();
        commandData.commandName = commandName;
        commandData.methodName = methodName;

        Enqueue(commandData);
    }

    public bool ContainMethod(string methodName) {

        foreach(var command in this) {

            if (command.methodName == methodName)
                return true;

        }

        return false;

    }

}