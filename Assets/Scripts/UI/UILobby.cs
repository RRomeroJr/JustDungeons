using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class UILobby : MonoBehaviour
{
    public Button buttonLobbyReady;
    public Button buttonLobbyStart;
    public Button buttonLobbyLeave;
    public DropdownField dropdownClass;
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
        dropdownClass = root.Q<DropdownField>("class-select");
        dropdownClass.choices = Resources.LoadAll<CombatClass>("").Select(x => x.name).ToList();
        playerList = new List<VisualElement>
        {
            root.Q<VisualElement>("player-1"),
            root.Q<VisualElement>("player-2"),
            root.Q<VisualElement>("player-3"),
            root.Q<VisualElement>("player-4")
        };
    }
}
