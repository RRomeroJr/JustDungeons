using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DapperDino;

public class PlayerGame : NetworkBehaviour
{
    [SyncVar]
    private string displayName = "Loading...";
    
    public override void OnStartClient()
    {
        DontDestroyOnLoad(this);
        GetComponent<Actor>().enabled = true;


        // Player Game no longer activates itself due to weirdness with the ReplacePlayerForConnection method
        // They have to be replaced by the new Player obj as it's created to no caue issues
        // See CustomNetworkManager.ReplacePlayer()

        if(!CustomNetworkManager.singleton.GamePlayers.Contains(this))
        {// Think about this check as, if not already handled by the CustomNetworkManager
        
            CustomNetworkManager.singleton.GamePlayers.Add(this);
            Debug.Log(gameObject.name + " added PlayerGame to GamePlayers");
        }

    }

    public override void OnStopClient()
    {
        // Player Game no longer activates itself due to weirdness with the ReplacePlayerForConnection method
        // They have to be replaced by the new Player obj as it's created to no caue issues
        // See CustomNetowrkManager.ReplacePlayer()

        if(CustomNetworkManager.singleton.GamePlayers.Contains(this))
        {// Think about this check as, if not already handled by the CustomNetworkManager
        
            CustomNetworkManager.singleton.GamePlayers.Remove(this);
        }
    }
    public void OnReplace()
    {
        CustomNetworkManager.singleton.GamePlayers.Remove(this);
    }

    [Server]
    public void SetDisplayName(string displayName)
    {
        this.displayName = displayName;
    }
}
