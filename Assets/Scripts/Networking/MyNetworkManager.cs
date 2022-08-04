using Mirror;

public class MyNetworkManager : NetworkManager
{

    private static MyNetworkManager _instance;
    public static MyNetworkManager instance{
        get {
            return _instance;
        }
    }

    public override void Start() {

        base.Start();

        if(_instance == null)
            _instance = this;

    }

}
