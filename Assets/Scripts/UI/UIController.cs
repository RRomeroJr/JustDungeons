using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using Mirror;

public class UIController : MonoBehaviour
{
    public UITitle uiTitle;
    public UILobby uiLobby;
    public VisualTreeAsset lobby;
    public VisualTreeAsset title;
    //GameObject localPlayer = NetworkClient.localPlayer.gameObject;

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
        uiLobby.enabled = false;
        uiTitle.enabled = true;
        uiTitle.buttonHostLobby.clicked += HostButtonPressed;
        uiTitle.buttonJoinIP.clicked += JoinLobbyButtonPressed;
    }

    void JoinLobby()
    {
        uiTitle.buttonHostLobby.clicked -= HostButtonPressed;
        uiTitle.buttonJoinIP.clicked -= JoinLobbyButtonPressed;
        uiTitle.enabled = false;
        uiLobby.enabled = true;
        //uiLobby.buttonLobbyStart.visible = false;
        /*buttonLobbyReady.clicked += ReadyButtonPressed;
        buttonLobbyStart.clicked += StartButtonPressed;*/
        uiLobby.buttonLobbyLeave.clicked += LeaveButtonPressed;
    }

    void HostButtonPressed()
    {
        networkManager.StartHost();
    }

    void JoinLobbyButtonPressed()
    {
        string ipAddress = uiTitle.fieldIP.text;

        if (ipAddress == "")
        {
            ipAddress = "localhost";
        }
        networkManager.networkAddress = ipAddress;
        networkManager.StartClient();
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
        //networkManager.RoomPlayers
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
        foreach (VisualElement player in uiLobby.playerList)
        {
            player.Q<Label>("player-name").text = "Waiting For Player...";
            player.Q<Label>("ready-status").text = string.Empty;
        }

        int i = 0;
        foreach (PlayerLobby player in networkManager.RoomPlayers)
        {
            uiLobby.playerList[i].Q<Label>("player-name").text = networkManager.RoomPlayers[i].DisplayName;
            uiLobby.playerList[i].Q<Label>("ready-status").text = networkManager.RoomPlayers[i].IsReady ?
                "<color=green>Ready</color>" :
                "<color=red>Not Ready</color>";
            i++;
        }
    }

    #endregion
}
