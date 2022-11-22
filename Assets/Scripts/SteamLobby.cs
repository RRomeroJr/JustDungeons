using Mirror;
using Steamworks;
using UnityEngine;


    public class SteamLobby : MonoBehaviour
    {

        protected Callback<LobbyCreated_t> lobbyCreated;
        protected Callback<GameLobbyJoinRequested_t> gameLobbyJoinRequested;
        protected Callback<LobbyEnter_t> lobbyEntered;

        private const string HostAddressKey = "HostAddress";

        private NetworkManager networkManager;
        
        private void Start()
        {
            networkManager = GetComponent<NetworkManager>();

            //if Steam isn't open on this machine don't even try to do this
            if (!SteamManager.Initialized) { return; }



            // Probably only need this part down here
            // When a lobby is created what do we call?     vvvvv
            lobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);

            gameLobbyJoinRequested = Callback<GameLobbyJoinRequested_t>.Create(OnGameLobbyJoinRequested);
            lobbyEntered = Callback<LobbyEnter_t>.Create(OnLobbyEntered);
        }

        public void HostLobby()
        {
            

            SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypeFriendsOnly, networkManager.maxConnections);
            //Triggers callback LobbyCreated_t?
        }
        
        private void OnLobbyCreated(LobbyCreated_t callback)
        {
            /*  
                This will still get called EVEN IF the lobby fails to be created
            */
            //if the call back result is not OK
            if (callback.m_eResult != EResult.k_EResultOK)
            {
                
                return;
            }

            networkManager.StartHost();
            //A Steam lobby can hold some data. The goal here is to save the 
            // host's Steam ID so that people can access it when trying to connect to the game

            SteamMatchmaking.SetLobbyData(
                new CSteamID(callback.m_ulSteamIDLobby),
                HostAddressKey,
                SteamUser.GetSteamID().ToString());
        } //CStreamID is the type for holding steam ids
         //Evey piece of data needs a key and a value (HostAddressKey, SteamUser.GetSteamID().ToString())

        private void OnGameLobbyJoinRequested(GameLobbyJoinRequested_t callback)
        {
            SteamMatchmaking.JoinLobby(callback.m_steamIDLobby);
        }

        private void OnLobbyEntered(LobbyEnter_t callback)
        {
            // When the client actually enters the lobby

            //If you are a HOST return
            if (NetworkServer.active) { return; }

            //Using this Steam ID get the data attached to this key (HostAddressKey)
            string hostAddress = SteamMatchmaking.GetLobbyData(
                new CSteamID(callback.m_ulSteamIDLobby),
                HostAddressKey);
            
            //Then using the data Steam stored for us set up Mirror things
            networkManager.networkAddress = hostAddress;
            networkManager.StartClient();

            
        }
    }
