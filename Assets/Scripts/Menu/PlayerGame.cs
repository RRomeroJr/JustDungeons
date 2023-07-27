using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DapperDino;

public class PlayerGame : NetworkBehaviour
{
    [SyncVar]
    private string displayName = "Loading...";

    private CustomNetworkManager room;
    private CustomNetworkManager Room
    {
        get
        {
            if (room != null)
            {
                return room;
            }
            return room = NetworkManager.singleton as CustomNetworkManager;
        }
    }

    public override void OnStartClient()
    {
        DontDestroyOnLoad(this);
        GetComponent<Actor>().enabled = true;
        Room.GamePlayers.Add(this);
        //GetComponent<ClickManager>().enabled = true;
    }

    public override void OnStopClient()
    {
        Room.GamePlayers.Remove(this);
    }

    [Server]
    public void SetDisplayName(string displayName)
    {
        this.displayName = displayName;
    }
}
