using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;


public class MainMenu : MonoBehaviour
{
    [SerializeField] private CustomNetworkManager networkManager;

    [Header("UI")]
    [SerializeField] private GameObject landingPagePanel = null;

    public enum SceneName
    {

    }

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void HostLobby()
    {
        networkManager.SteamHostLobby();
        //networkManager.StartHost();
        landingPagePanel.SetActive(false);
    }
}
