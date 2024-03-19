using DapperDino;
using Mirror;
using UnityEngine;
using UnityEngine.UIElements;
using Mirror.FizzySteam;
using Steamworks;

public class PlayerLobby : NetworkBehaviour
{
    [Header("UI")]
    private UIController uiController;
    private TextField playerSlot;
    private DropdownField classSelect;
    private DropdownField dungeonSelect;

    [SyncVar(hook = nameof(HandleDisplayNameChanged))]
    public string DisplayName = "Loading...";
    [SyncVar(hook = nameof(HandleReadyStatusChanged))]
    public bool IsReady = false;
    [SyncVar(hook = nameof(OnCombatClassSync))]
    public string combatClass;

    private bool isLeader;
    public bool IsLeader
    {
        set
        {
            isLeader = value;
        }
    }

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

    void Awake()
    {
        uiController = GameObject.Find("UIDocument").GetComponent<UIController>();

    }

    public override void OnStartAuthority()
    {
        CmdSetDisplayName(PlayerNameInput.DisplayName);
    }

    public override void OnStartClient()
    {
        DontDestroyOnLoad(this);
        Room.RoomPlayers.Add(this);
        playerSlot = uiController.uiLobby.playerList[Room.RoomPlayers.IndexOf(this)].Q<TextField>("player-name");

        // Subscribe to button events
        uiController.uiLobby.buttonLobbyReady.clicked += CmdReadyUp;
        uiController.uiLobby.buttonLobbyLeave.clicked += CmdLeaveGame;
        uiController.uiLobby.buttonLobbyStart.clicked += CmdStartGame;

        // Bind display name to textfield
        playerSlot.RegisterValueChangedCallback(OnPlayerNameChanged);

        if(isLocalPlayer){
            classSelect = uiController.uiLobby.dropdownClass;
            classSelect.RegisterValueChangedCallback(OnClassChanged);
            dungeonSelect = uiController.uiLobby.dropdownDungeon;
            // classSelect.RegisterValueChangedCallback(OnClassChanged);
            //Set Name to Steam name if on Steam
            if(NetworkManager.singleton.transport.GetType() == typeof(FizzySteamworks))
            {
                CmdSetDisplayName(SteamFriends.GetPersonaName());
            }
        }
        
        playerSlot.isReadOnly = false;
        // classSelect.index = 1;
        uiController.UpdateUI();

        // Hide start button if not host
        if (!isServer)
        {
            uiController.uiLobby.buttonLobbyStart.style.display = DisplayStyle.None;
        }

       
    }

    private void OnClassChanged(ChangeEvent<string> evt)
    {
        // if (isServer)
        // {
        //     combatClass = evt.newValue;
        //     if(CustomNetworkManager.singleton.playerInfo.ContainsKey(connectionToClient))
        //     {
        //         Debug.Log("Connection found! changing " + CustomNetworkManager.singleton.playerInfo[connectionToClient].name
        //          + "'s PlayerData.combatClass to " + evt.newValue);
        //         CustomNetworkManager.singleton.playerInfo[connectionToClient].combatClass = evt.newValue;
        //     }
        //     else
        //     {
        //         Debug.LogError("Could not NetworkConnectionToClient");
        //     }
        //     return;
        // }
        CmdSetCombatClass(evt.newValue);
    }

    private void OnPlayerNameChanged(ChangeEvent<string> evt)
    {
        if (isServer)
        {
            DisplayName = evt.newValue;
            if(CustomNetworkManager.singleton.playerInfo.ContainsKey(connectionToClient))
            {
                Debug.Log("Connection found! " + CustomNetworkManager.singleton.playerInfo[connectionToClient].name
                 + "'s PlayerData.name to " + evt.newValue);
                CustomNetworkManager.singleton.playerInfo[connectionToClient].name = evt.newValue;
            }
            else
            {
                Debug.LogError("Could not NetworkConnectionToClient");
            }
            return;
        }
        CmdSetDisplayName(evt.newValue);
    }

    public override void OnStopClient()
    {
        Room.RoomPlayers.Remove(this);
        uiController.UpdateUI();
    }

    public void HandleReadyStatusChanged(bool oldValue, bool newValue) => uiController.UpdateUI();
    public void HandleDisplayNameChanged(string oldValue, string newValue) => uiController.UpdateUI();
    public void OnCombatClassSync(string oldValue, string newValue)
    {
        if (isServer)
        {
            // combatClass = newValue;
            if(CustomNetworkManager.singleton.playerInfo.ContainsKey(connectionToClient))
            {
                Debug.Log("Connection found! changing " + CustomNetworkManager.singleton.playerInfo[connectionToClient].name
                 + "'s PlayerData.combatClass to " + newValue);
                CustomNetworkManager.singleton.playerInfo[connectionToClient].combatClass = newValue;
            }
            else
            {
                Debug.LogError("Could not NetworkConnectionToClient");
            }

        }
    }

    [Command]
    private void CmdSetDisplayName(string displayName)
    {
        DisplayName = displayName;
    }

    [Command]
    private void CmdSetCombatClass(string c)
    {
        combatClass = c;
    }
    [ClientRpc]
    private void OnCombatClassSync(string c)
    {
        combatClass = c;
    }

    [Command]
    public void CmdReadyUp()
    {
        IsReady = !IsReady;
    }

    [Command]
    public void CmdStartGame()
    {
        if (!isServer)
        {
            return;
        }
    
        GameManager.instance.dungeonScalingLevel = uiController.uiLobby.dungeonLevel.index >= 0 ? uiController.uiLobby.dungeonLevel.index : 0;
       
        if(dungeonSelect.text == ""){
            Room.StartGame();
        }
        else
        {
            Room.StartGame(dungeonSelect.text);
        }
    }

    [Command]
    public void CmdLeaveGame()
    {
        if (isServer)
        {
            Room.StopHost();
            return;
        }
        Room.StopClient();
    }
}
