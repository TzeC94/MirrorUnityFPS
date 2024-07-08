using System;
public static partial class ConsoleCommandManager {

	public struct Command{
		public string name;
		public Action callAction;
		public bool isServerCommand;
	}

	public static Command[] command = {
		new Command{
			name = "RestoreHealth",
			callAction = BaseCombatScript.RestoreAllHealth,
            isServerCommand = false
        }
	};
}