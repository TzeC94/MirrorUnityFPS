using System;
public static partial class ConsoleCommandManager {

	public struct Command{
		public string name;
		public Action callAction;
	}

	public static Command[] command = {
		new Command{
			name = "RestoreHealth",
			callAction = BaseCombatScript.RestoreAllHealth
		}
	};
}