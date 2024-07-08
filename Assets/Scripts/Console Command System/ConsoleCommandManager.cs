using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Reflection;
using System.Linq;
using System.Text;
using Mirror;
using Cysharp;
using Cysharp.Text;

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

                commandBuilderDic[method.DeclaringType.Name].Add(commandAttribute.commandName, method.Name, commandAttribute.serverCommand);

                Debug.LogFormat("[Console Command Builder] Add New Function Value {0}", method.Name);

            }


        }

        #endregion

        #region Generation Class Data

        //Clear dictionary
        commandDic.Clear();

        StringBuilder stringBuilder = new StringBuilder();

        stringBuilder.AppendLine("using System;");

        stringBuilder.AppendLine("");

        //Start with class
        stringBuilder.AppendLine("public static partial class ConsoleCommandManager {");

        stringBuilder.AppendLine("");

        stringBuilder.AppendLine("\tpublic struct Command{");
        stringBuilder.AppendLine("\t\tpublic string name;");
        stringBuilder.AppendLine("\t\tpublic Action callAction;");
        stringBuilder.AppendLine("\t}");

        stringBuilder.AppendLine("");

        stringBuilder.AppendLine("\tpublic static Command[] command = {");

        stringBuilder.AppendLine("");

        int counter = 0;

        foreach (var commandBuilder in commandBuilderDic) {

            //Expand the function
            foreach (var command in commandBuilder.Value) {

                stringBuilder.AppendLine("\t\tnew Command{");

                //Function name
                stringBuilder.AppendLine("\t\t\tname = \u0022" + command.commandName + "\u0022,");

                //Body
                stringBuilder.AppendLine("\t\t\tcallAction = " + commandBuilder.Key + "." + command.methodName);

                //Server
                stringBuilder.AppendLine("\t\t\tisServerCommand = " + command.serverOnly);

                //End
                stringBuilder.AppendLine("\t\t}");

                //Expand the dictionary for caching
                commandDic.Add(command.commandName, counter);

                counter++;
            }

        }

        stringBuilder.AppendLine("\t};");

        stringBuilder.AppendLine("");

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

    public static Dictionary<string, int> commandDic = new Dictionary<string, int>();

    public static Action<string> onCommandEnter;

    public static void TriggerCommand(string commandName) {

        InitialiseMessage();

        //Lets build a message for server
        var localPlayer = GameManagerBase.LocalPlayer;

        CommandMessage commandMessage;
        commandMessage.playerID = localPlayer != null? localPlayer.netId : 0;
        commandMessage.commandName = commandName;

        //Find from the dictionary
        if (commandDic.ContainsKey(commandName)) {

            var commandInfo = command[commandDic[commandName]];

            //If is server but calling from client, lets build a message and send to server
            if (commandInfo.isServerCommand && GameManagerBase.instance.isClient) {

                //Send the command to server
                NetworkClient.Send(commandMessage);

                return;
            }
        }

        //Proceed to call the command if we can call it directly
        TriggerCommand(commandMessage);
    }

    public static void TriggerCommand(CommandMessage commandMessage) {

        InitialiseMessage();

        //Find from the dictionary
        if (commandDic.ContainsKey(commandMessage.commandName)) {

            var commandInfo = command[commandDic[commandMessage.commandName]];

            commandInfo.callAction.Invoke();

            onCommandEnter?.Invoke(commandMessage.commandName);
        }

        const string ERROR = "Failed execute command {0}";
        onCommandEnter?.Invoke(ZString.Format(ERROR, commandMessage.commandName));
    }
}

[AttributeUsage(AttributeTargets.Method)]
public class ConCommandAttribute : Attribute 
{
    public string commandName;
    public bool serverCommand;

    /// <summary>
    /// Please make sure you're adding this in a partial class only
    /// </summary>
    /// <param name="commandName"></param>
    public ConCommandAttribute(string commandName, bool serverCommand = false)
    {
        this.commandName = commandName;
        this.serverCommand = serverCommand;
    }
}

public struct CommandData {

    public string commandName;
    public string methodName;
    public bool serverOnly;

}

public class CommandQueue : Queue<CommandData> {

    public void Add(string commandName, string methodName, bool serverCommand) {

        CommandData commandData = new CommandData();
        commandData.commandName = commandName;
        commandData.methodName = methodName;
        commandData.serverOnly = serverCommand;

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

public struct CommandMessage : NetworkMessage {

    public uint playerID;
    public string commandName;

}