using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DapperDino;
using System.Collections.ObjectModel;
using Steamworks;

/*
	Documentation: https://mirror-networking.gitbook.io/docs/components/network-manager
	API Reference: https://mirror-networking.com/docs/api/Mirror.NetworkManager.html
*/

public class CustomNetworkManager : NetworkManager
{
    // Overrides the base singleton so we don't
    // have to cast to this type everywhere.
    public static new CustomNetworkManager singleton { get; private set; }
    public UIManager uiManager;
    [SerializeField] private int minPlayers = 1;
    [Scene][SerializeField] private string menuScene = string.Empty;

    [Header("Room")]
    [SerializeField] private PlayerLobby roomPlayerPrefab = null;

    [Header("Game")]
    [SerializeField] private PlayerGame gamePlayerPrefab = null;
    [SerializeField] private GameObject playerSpawnSystem = null;
    [SerializeField] private GameObject roundSystem = null;

    public static event Action OnClientConnected;
    public static event Action OnClientDisconnected;
    public ObservableCollection<PlayerLobby> RoomPlayers { get; } = new();
    public ObservableCollection<PlayerGame> GamePlayers { get; } = new();
    protected Callback<LobbyCreated_t> lobbyCreated;
    protected Callback<GameLobbyJoinRequested_t> gameLobbyJoinRequested;
    protected Callback<LobbyEnter_t> lobbyEntered;
    public Dictionary<NetworkConnectionToClient, PlayerData> playerInfo = new Dictionary<NetworkConnectionToClient, PlayerData>();

    private const string HostAddressKey = "HostAddress";

    #region Unity Callbacks

    public override void OnValidate()
    {
        base.OnValidate();
    }
    

    /// <summary>
    /// Runs on both Server and Client
    /// Networking is NOT initialized when this fires
    /// </summary>
    public override void Awake()
    {
        base.Awake();
    }

    /// <summary>
    /// Runs on both Server and Client
    /// Networking is NOT initialized when this fires
    /// </summary>
    public override void Start()
    {
        // When a lobby is created what do we call?     vvvvv
        lobbyCreated = Callback<LobbyCreated_t>.Create(SteamOnLobbyCreated);

        gameLobbyJoinRequested = Callback<GameLobbyJoinRequested_t>.Create(SteamOnGameLobbyJoinRequested);
        lobbyEntered = Callback<LobbyEnter_t>.Create(SteamOnLobbyEntered);

        singleton = this;
        base.Start();
    }

    /// <summary>
    /// Runs on both Server and Client
    /// </summary>
    public override void LateUpdate()
    {
        base.LateUpdate();
    }
   
    public override void Update()
    {
        if(Input.GetKeyDown("n"))
        {
            if(playerInfo.Count == 0)
            {
                Debug.Log("No entries in playerInfo");
                MsgBox.DisplayMsg("No entries in playerInfo");
            }
            else
            {
                foreach(KeyValuePair<NetworkConnectionToClient, PlayerData> entry in playerInfo)
                {
                    Debug.Log(entry.Value.name + ": " + entry.Value.combatClass);
                }
                Debug.Log("Number of GamePlayers: " + GamePlayers.Count);
                MsgBox.DisplayMsg("Number of GamePlayers: " + GamePlayers.Count);
            }

        }
        if(Input.GetKeyDown(","))
        {
            if(playerInfo.Count == 0)
            {
                Debug.Log("No entries in playerInfo");
            }
            foreach(KeyValuePair<NetworkConnectionToClient, PlayerData> entry in playerInfo)
            {
                Respawn(entry.Key);
            }
        }

    }

    /// <summary>
    /// Runs on both Server and Client
    /// </summary>
    public override void OnDestroy()
    {
        base.OnDestroy();
    }

    #endregion

    #region Start & Stop

    /// <summary>
    /// Set the frame rate for a headless server.
    /// <para>Override if you wish to disable the behavior or set your own tick rate.</para>
    /// </summary>
    public override void ConfigureHeadlessFrameRate()
    {
        base.ConfigureHeadlessFrameRate();
    }

    /// <summary>
    /// called when quitting the application by closing the window / pressing stop in the editor
    /// </summary>
    public override void OnApplicationQuit()
    {
        base.OnApplicationQuit();
    }

    #endregion

    #region Scene Management

    /// <summary>
    /// This causes the server to switch scenes and sets the networkSceneName.
    /// <para>Clients that connect to this server will automatically switch to this scene. This is called automatically if onlineScene or offlineScene are set, but it can be called from user code to switch scenes again while the game is in progress. This automatically sets clients to be not-ready. The clients must call NetworkClient.Ready() again to participate in the new scene.</para>
    /// </summary>
    /// <param name="newSceneName"></param>
    public override void ServerChangeScene(string newSceneName)
    {
        // From menu to game
        base.ServerChangeScene(newSceneName);
    }

    /// <summary>
    /// Called from ServerChangeScene immediately before SceneManager.LoadSceneAsync is executed
    /// <para>This allows server to do work / cleanup / prep before the scene changes.</para>
    /// </summary>
    /// <param name="newSceneName">Name of the scene that's about to be loaded</param>
    public override void OnServerChangeScene(string newSceneName) { }

    /// <summary>
    /// Called on the server when a scene is completed loaded, when the scene load was initiated by the server with ServerChangeScene().
    /// </summary>
    /// <param name="sceneName">The name of the new scene.</param>
    public override void OnServerSceneChanged(string sceneName)
    {
        // for (int i = RoomPlayers.Count - 1; i >= 0; i--)
        // {
        //     var conn = RoomPlayers[i].connectionToClient;
        //     Transform startPos = GetStartPosition();
        //     // var gameplayerInstance = Instantiate(gamePlayerPrefab);
        //     var gameplayerInstance = startPos != null
        //         ? Instantiate(gamePlayerPrefab, startPos.position, startPos.rotation)
        //         : Instantiate(gamePlayerPrefab);
        //     gameplayerInstance.SetDisplayName(RoomPlayers[i].DisplayName);
        //     var playerActor = gameplayerInstance.GetComponent<Actor>();
        //     playerActor.ActorName = RoomPlayers[i].DisplayName;
        //     playerActor.combatClass = Resources.Load<CombatClass>(RoomPlayers[i].combatClass);

        //     NetworkServer.Destroy(conn.identity.gameObject);

        //     NetworkServer.ReplacePlayerForConnection(conn, gameplayerInstance.gameObject);
        // }
        // if(SceneManager.GetActiveScene().path == menuScene)
        // {
        //     return;
        // }
        foreach(KeyValuePair<NetworkConnectionToClient, PlayerData> entry in playerInfo)
        {
            var gameplayerInstance = NewPlayerObj(entry.Value);

            NetworkServer.Destroy(entry.Key.identity.gameObject);
            NetworkServer.ReplacePlayerForConnection(entry.Key, gameplayerInstance.gameObject);
            
        }
        // RoomPlayers.Clear();
       
    }

    /// <summary>
    /// Called from ClientChangeScene immediately before SceneManager.LoadSceneAsync is executed
    /// <para>This allows client to do work / cleanup / prep before the scene changes.</para>
    /// </summary>
    /// <param name="newSceneName">Name of the scene that's about to be loaded</param>
    /// <param name="sceneOperation">Scene operation that's about to happen</param>
    /// <param name="customHandling">true to indicate that scene loading will be handled through overrides</param>
    public override void OnClientChangeScene(string newSceneName, SceneOperation sceneOperation, bool customHandling) { }

    /// <summary>
    /// Called on clients when a scene has completed loaded, when the scene load was initiated by the server.
    /// <para>Scene changes can cause player objects to be destroyed. The default implementation of OnClientSceneChanged in the NetworkManager is to add a player object for the connection if no player object exists.</para>
    /// </summary>
    public override void OnClientSceneChanged()
    {
        base.OnClientSceneChanged();
    }

    #endregion

    #region Server System Callbacks

    /// <summary>
    /// Called on the server when a new client connects.
    /// <para>Unity calls this on the Server when a Client connects to the Server. Use an override to tell the NetworkManager what to do when a client connects to the server.</para>
    /// </summary>
    /// <param name="conn">Connection from client.</param>
    public override void OnServerConnect(NetworkConnectionToClient conn) 
    {
        if (numPlayers >= maxConnections)
        {
            conn.Disconnect();
            return;
        }

        if (SceneManager.GetActiveScene().path != menuScene)
        {
            conn.Disconnect();
            return;
        }
    }

    /// <summary>
    /// Called on the server when a client is ready.
    /// <para>The default implementation of this function calls NetworkServer.SetClientReady() to continue the network setup process.</para>
    /// </summary>
    /// <param name="conn">Connection from client.</param>
    public override void OnServerReady(NetworkConnectionToClient conn)
    {
        base.OnServerReady(conn);
    }

    /// <summary>
    /// Called on the server when a client adds a new player with ClientScene.AddPlayer.
    /// <para>The default implementation for this function creates a new player object from the playerPrefab.</para>
    /// </summary>
    /// <param name="conn">Connection from client.</param>
    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        if (SceneManager.GetActiveScene().path == menuScene)
        {
            bool isLeader = RoomPlayers.Count == 0;
            PlayerLobby roomPlayerInstance = Instantiate(roomPlayerPrefab);
            roomPlayerInstance.IsLeader = isLeader;
            NetworkServer.AddPlayerForConnection(conn, roomPlayerInstance.gameObject);

            PlayerData playerData = new PlayerData();

            playerData.name = roomPlayerInstance.DisplayName;
            playerData.combatClass = roomPlayerInstance.combatClass;

            playerInfo[conn] = playerData;
        }
    }

    /// <summary>
    /// Called on the server when a client disconnects.
    /// <para>This is called on the Server when a Client disconnects from the Server. Use an override to decide what should happen when a disconnection is detected.</para>
    /// </summary>
    /// <param name="conn">Connection from client.</param>
    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        if (conn.identity != null)
        {
            var player = conn.identity.GetComponent<PlayerLobby>();
            RoomPlayers.Remove(player);
            
            if(playerInfo.ContainsKey(conn))
            {
                playerInfo.Remove(conn);
            }
        }
        base.OnServerDisconnect(conn);
    }

    /// <summary>
    /// Called on server when transport raises an exception.
    /// <para>NetworkConnection may be null.</para>
    /// </summary>
    /// <param name="conn">Connection of the client...may be null</param>
    /// <param name="exception">Exception thrown from the Transport.</param>
    public override void OnServerError(NetworkConnectionToClient conn, Exception exception) { }

    #endregion

    #region Client System Callbacks

    /// <summary>
    /// Called on the client when connected to a server.
    /// <para>The default implementation of this function sets the client as ready and adds a player. Override the function to dictate what happens when the client connects.</para>
    /// </summary>
    public override void OnClientConnect()
    {
        base.OnClientConnect();
        OnClientConnected?.Invoke();
    }

    /// <summary>
    /// Called on clients when disconnected from a server.
    /// <para>This is called on the client when it disconnects from the server. Override this function to decide what happens when the client disconnects.</para>
    /// </summary>
    public override void OnClientDisconnect()
    {
        base.OnClientDisconnect();
        OnClientDisconnected?.Invoke();
    }

    /// <summary>
    /// Called on clients when a servers tells the client it is no longer ready.
    /// <para>This is commonly used when switching scenes.</para>
    /// </summary>
    public override void OnClientNotReady() { }

    /// <summary>
    /// Called on client when transport raises an exception.</summary>
    /// </summary>
    /// <param name="exception">Exception thrown from the Transport.</param>
    public override void OnClientError(Exception exception) { }

    #endregion

    #region Start & Stop Callbacks

    // Since there are multiple versions of StartServer, StartClient and StartHost, to reliably customize
    // their functionality, users would need override all the versions. Instead these callbacks are invoked
    // from all versions, so users only need to implement this one case.

    /// <summary>
    /// This is invoked when a host is started.
    /// <para>StartHost has multiple signatures, but they all cause this hook to be called.</para>
    /// </summary>
    public override void OnStartHost() { }

    /// <summary>
    /// This is invoked when a server is started - including when a host is started.
    /// <para>StartServer has multiple signatures, but they all cause this hook to be called.</para>
    /// </summary>
    public override void OnStartServer(){
        List<GameObject> temp  = Resources.LoadAll<GameObject>("Networked/").ToList();
        foreach (GameObject go in temp){
            spawnPrefabs.Add(go);
        }
    }
    // => spawnPrefabs = Resources.LoadAll<GameObject>("").ToList();

    /// <summary>
    /// This is invoked when the client is started.
    /// </summary>
    public override void OnStartClient()
    {
        var spawnablePrefabs = Resources.LoadAll<GameObject>("Networked/").ToList();

        foreach (var prefab in spawnablePrefabs)
        {
            NetworkClient.RegisterPrefab(prefab);
        }
    }

    /// <summary>
    /// This is called when a host is stopped.
    /// </summary>
    public override void OnStopHost() { }

    /// <summary>
    /// This is called when a server is stopped - including when a host is stopped.
    /// </summary>
    public override void OnStopServer()
    {
        RoomPlayers.Clear();
    }

    /// <summary>
    /// This is called when a client is stopped.
    /// </summary>
    public override void OnStopClient() { }

    #endregion

    private bool IsReadyToStart()
    {
        if (numPlayers < minPlayers)
        {
            return false;
        }

        foreach (var player in RoomPlayers)
        {
            if (!player.IsReady)
            {
                return false;
            }
        }
        return true;
    }

    public void StartGame()
    {
        if (SceneManager.GetActiveScene().path == menuScene)
        {
            if (!IsReadyToStart())
            {
                return;
            }
            ServerChangeScene("MMO dungeon");
        }
    }
    public void StartGame(string _sceneName)
    {
        if (SceneManager.GetActiveScene().path == menuScene)
        {
            if (!IsReadyToStart())
            {
                return;
            }
            ServerChangeScene(_sceneName);
        }
    }
    public void SteamHostLobby()
    {
        

        SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypeFriendsOnly, maxConnections);
        //Triggers callback LobbyCreated_t?
    }
    
    private void SteamOnLobbyCreated(LobbyCreated_t callback)
    {
        /*  
            This will still get called EVEN IF the lobby fails to be created
        */
        //if the call back result is not OK
        if (callback.m_eResult != EResult.k_EResultOK)
        {
            
            return;
        }
        Debug.Log("SteamOnLobbyCreated");
        StartHost();
        //A Steam lobby can hold some data. The goal here is to save the 
        // host's Steam ID so that people can access it when trying to connect to the game

        SteamMatchmaking.SetLobbyData(
            new CSteamID(callback.m_ulSteamIDLobby),
            HostAddressKey,
            SteamUser.GetSteamID().ToString());
    } 
    //CStreamID is the type for holding steam ids
    //Every piece of data needs a key and a value (HostAddressKey, SteamUser.GetSteamID().ToString())

    private void SteamOnGameLobbyJoinRequested(GameLobbyJoinRequested_t callback)
    {
        SteamMatchmaking.JoinLobby(callback.m_steamIDLobby);
    }

    private void SteamOnLobbyEntered(LobbyEnter_t callback)
    {
        // When the client actually enters the lobby

        //If you are a HOST return
        if (NetworkServer.active) { return; }

        //Using this Steam ID get the data attached to this key (HostAddressKey)
        string hostAddress = SteamMatchmaking.GetLobbyData(
            new CSteamID(callback.m_ulSteamIDLobby),
            HostAddressKey);
        
        //Then using the data Steam stored for us set up Mirror things
        networkAddress = hostAddress;
        StartClient();
        
    }
    /// <summary>
    /// Pass new playerObj and connection should be made with that conn and the new object
    /// </summary>
    public void ReplacePlayer(NetworkConnectionToClient conn, GameObject newPlayerObj)
    {
        // Cache a reference to the current player object
        GameObject oldPlayer = conn.identity.gameObject;

        var oldPlayerIndex = GamePlayers.IndexOf(oldPlayer.GetComponent<PlayerGame>());
        if(oldPlayerIndex == -1)
        {
            Debug.LogError("oldPlayerIndex not found: " + oldPlayer.gameObject.name);
        }
        else
        {
            
            // Instantiate the new player object and broadcast to clients
            // Include true for keepAuthority paramater to prevent ownership change
            NetworkServer.ReplacePlayerForConnection(conn, newPlayerObj, true);

            // Replacing PlayerGame in GamePlayers with the new PlayerGame
            // Really important to do this because ReplacePlayerForConnection needs
            // a delay to work properly, causing 2 players for the same person to exist
            // briefly

            // Code that is listening to this collection rely on finding the localPlayer
            // But localPlayer doesn't get set untill ReplacePlayerForConnection is finished
            // So this must be done after
            GamePlayers[oldPlayerIndex] = newPlayerObj.GetComponent<PlayerGame>();

            // Remove the previous player object that's now been replaced
            // Delay is required to allow replacement to complete.
            Destroy(oldPlayer, 0.1f);

        }

    }
    public GameObject NewPlayerObj(PlayerData _pd)
    {
        Transform startPos = GetStartPosition();
        var gameplayerInstance = startPos != null
            ? Instantiate(gamePlayerPrefab, startPos.position, startPos.rotation)
            : Instantiate(gamePlayerPrefab);
        gameplayerInstance.SetDisplayName(_pd.name);
        var playerActor = gameplayerInstance.GetComponent<Actor>();
        playerActor.ActorName = _pd.name;
        // 2/6/24 the default class is set by setting it in the Player 1 prefab
        playerActor.combatClass ??= Resources.Load<CombatClass>(_pd.combatClass);
        

        return gameplayerInstance.gameObject;
    }
    public void Respawn(NetworkConnectionToClient conn)
    {
        // if(playerInfo.ContainsKey(conn))
        // {
        //     ReplacePlayer(conn, NewPlayerObj(playerInfo[conn]));
        // }
        conn.identity.transform.position = GetStartPosition().position;
        conn.identity.GetComponent<Actor>().Revive();
    }

}
