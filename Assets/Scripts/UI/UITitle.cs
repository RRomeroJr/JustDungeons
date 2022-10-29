using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UITitle : MonoBehaviour
{
    public Button buttonHostLobby;
    public Button buttonJoinLobby;
    public Button buttonOptions;
    public Button buttonQuit;
    public Button buttonQuitIP;
    public Button buttonJoinIP;
    public TextField fieldIP;
    public UIDocument document;
    public VisualTreeAsset title;
    public VisualElement windowIP;

    void Awake()
    {
        document = GetComponent<UIDocument>();
    }

    void OnEnable()
    {
        document.visualTreeAsset = title;
        var root = document.rootVisualElement;

        buttonHostLobby = root.Q<Button>("button-host");
        buttonJoinLobby = root.Q<Button>("button-join");
        buttonOptions = root.Q<Button>("button-options");
        buttonQuit = root.Q<Button>("button-quit");
        windowIP = root.Q<VisualElement>("join-game-container");
        buttonQuitIP = root.Q<Button>("button-quit-ip");
        buttonJoinIP = root.Q<Button>("button-join-ip");
        fieldIP = root.Q<TextField>("field-enter-ip");

        windowIP.visible = false;

        buttonHostLobby.clicked += HostButtonPressed;
        buttonJoinLobby.clicked += JoinButtonPressed;
        buttonOptions.clicked += OptionsButtonPressed;
        buttonQuit.clicked += QuitButtonPressed;
        buttonQuitIP.clicked += QuitIPButtonPressed;
        buttonJoinIP.clicked += JoinIPButtonPressed;
    }

    void OnDisable()
    {
        buttonHostLobby.clicked -= HostButtonPressed;
        buttonJoinLobby.clicked -= JoinButtonPressed;
        buttonOptions.clicked -= OptionsButtonPressed;
        buttonQuit.clicked -= QuitButtonPressed;
        buttonQuitIP.clicked -= QuitIPButtonPressed;
        buttonJoinIP.clicked -= JoinIPButtonPressed;
    }

    void HostButtonPressed()
    {
    }

    void JoinButtonPressed()
    {
        windowIP.visible = true;
    }

    void OptionsButtonPressed()
    {
    }

    void QuitButtonPressed()
    {
        Application.Quit();
    }

    void QuitIPButtonPressed()
    {
        windowIP.visible = false;
    }

    void JoinIPButtonPressed()
    {
    }
}
