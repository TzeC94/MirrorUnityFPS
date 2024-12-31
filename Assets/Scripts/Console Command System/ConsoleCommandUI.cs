using UnityEngine;
using TMPro;
using Cysharp.Text;

public class ConsoleCommandUI : UIBase {

    public static ConsoleCommandUI Instance { get { return _Instance; } }
    private static ConsoleCommandUI _Instance;

    public TextMeshProUGUI commandTextField;

    private void Awake() {

        if (_Instance != null)
            return;

        _Instance = this;

        ConsoleCommandManager.onCommandEnter = ReceiveCommand;

        Init();
    }

    private void OnDestroy() {

        _Instance = null;

    }

    public void SendCommand(TMP_InputField command) {

        ConsoleCommandManager.TriggerCommand(command.text);

        //Clear it
        command.text = "";

    }

    private void ReceiveCommand(string message) {

        string currentText = commandTextField.text;

        commandTextField.text = ZString.Concat(currentText, "\n" + message);

    }
}
