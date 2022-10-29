using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UILobby : MonoBehaviour
{
    public Button buttonLobbyReady;
    public Button buttonLobbyStart;
    public Button buttonLobbyLeave;
    public List<VisualElement> playerList;
    public UIDocument document;
    public VisualTreeAsset lobby;

    void Awake()
    {
        document = GetComponent<UIDocument>();
    }

    void OnEnable()
    {
        document.visualTreeAsset = lobby;
        var root = document.rootVisualElement;

        buttonLobbyReady = root.Q<Button>("button-ready");
        buttonLobbyStart = root.Q<Button>("button-start");
        buttonLobbyLeave = root.Q<Button>("button-leave");
        playerList = new List<VisualElement>();
        playerList.Add(root.Q<VisualElement>("player-1"));
        playerList.Add(root.Q<VisualElement>("player-2"));
        playerList.Add(root.Q<VisualElement>("player-3"));
        playerList.Add(root.Q<VisualElement>("player-4"));

        //playerList[0][0] = "help";
    }
}
