using UnityEngine;
using UnityEngine.UIElements;

public class UIController : MonoBehaviour
{
    public UITitle uiTitle;
    public UILobby uiLobby;
    public VisualTreeAsset lobby;
    public VisualTreeAsset title;

    [SerializeField] private CustomNetworkManager networkManager;

    void OnEnable()
    {
        CustomNetworkManager.OnClientConnected += HandleClientConnected;
        CustomNetworkManager.OnClientDisconnected += HandleClientDisconnected;
    }

    private void OnDisable()
    {
        CustomNetworkManager.OnClientConnected -= HandleClientConnected;
        CustomNetworkManager.OnClientDisconnected -= HandleClientDisconnected;
    }

    void Start()
    {
        uiTitle = GetComponent<UITitle>();
        uiLobby = GetComponent<UILobby>();
        EnterTitle();
    }

    void EnterTitle()
    {
        // Enable title UI
        uiLobby.enabled = false;
        uiTitle.enabled = true;

        // Subscribe main menu buttons
        uiTitle.buttonHostLobby.clicked += HostButtonPressed;
        uiTitle.buttonJoinIP.clicked += JoinLobbyButtonPressed;
    }

    void JoinLobby()
    {
        // Unsubscribe main menu buttons
        uiTitle.buttonHostLobby.clicked -= HostButtonPressed;
        uiTitle.buttonJoinIP.clicked -= JoinLobbyButtonPressed;

        // Enable lobby UI
        uiTitle.enabled = false;
        uiLobby.enabled = true;
        uiLobby.buttonLobbyLeave.clicked += LeaveButtonPressed;
    }

    void HostButtonPressed()
    {
        networkManager.SteamHostLobby();
        //networkManager.StartHost();
    }

    void JoinLobbyButtonPressed()
    {
        string ipAddress = uiTitle.fieldIP.text;

        if (ipAddress == "")
        {
            ipAddress = "localhost";
        }
        Debug.LogError("Join button not yet implemented!");

        //networkManager.networkAddress = ipAddress;
        //networkManager.StartClient();
    }

    private void HandleClientConnected()
    {
        uiTitle.buttonJoinIP.SetEnabled(false);
        JoinLobby();
    }

    private void HandleClientDisconnected()
    {
        uiTitle.buttonJoinIP.SetEnabled(true);
    }

    #region LobbyMethods

    void ReadyButtonPressed()
    {

    }

    void StartButtonPressed()
    {

    }

    void LeaveButtonPressed()
    {
        //networkManager.StopHost();
        EnterTitle();
    }

    public void UpdateUI()
    {
        int i = 0;
        // Set name and ready status of players
        foreach (PlayerLobby player in networkManager.RoomPlayers)
        {
            uiLobby.playerList[i].Q<TextField>("player-name").value = player.DisplayName;
            uiLobby.playerList[i].Q<Label>("ready-status").text =
                player.IsReady ? "<color=green>Ready</color>" : "<color=red>Not Ready</color>";
            i++;
        }

        // TODO: 4 is hardcoded, but will need to change if we decide to support more players
        // Set empty slots to waiting
        for (int j = i; j < 4; j++)
        {
            uiLobby.playerList[j].Q<TextField>("player-name").value = "Waiting For Player...";
            uiLobby.playerList[j].Q<Label>("ready-status").text = string.Empty;
        }
    }

    #endregion
}
