using UnityEngine;
using Mirror;

public static partial class ConsoleCommandManager
{
    private static bool initialisedMessage = false;

    private static void InitialiseMessage() {

        if (initialisedMessage) return;

        initialisedMessage = true;

        NetworkServer.RegisterHandler<CommandMessage>(OnCommandReceived);

    }

    private static void OnCommandReceived(NetworkConnectionToClient client, CommandMessage command) {

        TriggerCommand(command);

    }
}
