using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DapperDino;

public class PlayerLobby : NetworkBehaviour
{
    [Header("UI")]
    private UIController uiController;
    [SerializeField] private TMP_Text[] playerNameTexts = new TMP_Text[4];
    [SerializeField] private TMP_Text[] playerReadyTexts = new TMP_Text[4];
    [SerializeField] private Button startGameButton = null;

    [SyncVar(hook = nameof(HandleDisplayNameChanged))]
    public string DisplayName = "Loading...";
    [SyncVar(hook = nameof(HandleReadyStatusChanged))]
    public bool IsReady = false;

    private bool isLeader;
    public bool IsLeader
    {
        set
        {
            isLeader = value;
            startGameButton.gameObject.SetActive(value);
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
        uiController.uiLobby.buttonLobbyReady.clicked += CmdReadyUp;
        uiController.uiLobby.buttonLobbyLeave.clicked += CmdLeaveGame;
        uiController.uiLobby.buttonLobbyStart.clicked += CmdStartGame;

        uiController.UpdateUI();
    }

    public override void OnStopClient()
    {
        Room.RoomPlayers.Remove(this);
        uiController.UpdateUI();
    }

    public void HandleReadyStatusChanged(bool oldValue, bool newValue) => uiController.UpdateUI();
    public void HandleDisplayNameChanged(string oldValue, string newValue) => uiController.UpdateUI();

    /*private void UpdateDisplay()
    {
        for (int i = 0; i < playerNameTexts.Length; i++)
        {
            playerNameTexts[i].text = "Waiting For Player...";
            playerReadyTexts[i].text = string.Empty;
        }

        for (int i = 0; i < Room.RoomPlayers.Count; i++)
        {
            playerNameTexts[i].text = Room.RoomPlayers[i].DisplayName;
            playerReadyTexts[i].text = Room.RoomPlayers[i].IsReady ?
                "<color=green>Ready</color>" :
                "<color=red>Not Ready</color>";
        }
    }*/

    public void HandleReadyToStart(bool readyToStart)
    {
        if (!isLeader) { return; }
        startGameButton.interactable = readyToStart;
    }

    [Command]
    private void CmdSetDisplayName(string displayName)
    {
        DisplayName = displayName;
    }

    [Command]
    public void CmdReadyUp()
    {
        IsReady = !IsReady;
        Room.NotifyPlayersOfReadyState();
    }

    [Command]
    public void CmdStartGame()
    {
        if (Room.RoomPlayers[0].connectionToClient != connectionToClient)
        {
            return;
        }
        Room.StartGame();
    }

    [Command]
    public void CmdLeaveGame()
    {
        if (Room.RoomPlayers[0].connectionToClient == connectionToClient)
        {
            Room.StopHost();
            return;
        }
        Room.StopClient();
    }
}
