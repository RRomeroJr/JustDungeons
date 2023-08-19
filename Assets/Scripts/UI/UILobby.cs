using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using System.IO;

public class UILobby : MonoBehaviour
{
    public Button buttonLobbyReady;
    public Button buttonLobbyStart;
    public Button buttonLobbyLeave;
    public DropdownField dropdownClass;
    public DropdownField dropdownDungeon;
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
        dropdownDungeon = root.Q<DropdownField>("dungeon-select");
        dropdownDungeon.choices = getAllSceneNames();
        playerList = new List<VisualElement>
        {
            root.Q<VisualElement>("player-1"),
            root.Q<VisualElement>("player-2"),
            root.Q<VisualElement>("player-3"),
            root.Q<VisualElement>("player-4")
        };
    }

    List<string> getAllSceneNames()
    {
        List<string> excluded = new List<string>();
        excluded.Add("TitleScene");
        List<string> sceneNames = new List<string>();
        for(int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string _name = SceneUtility.GetScenePathByBuildIndex(i);
            _name = Path.GetFileNameWithoutExtension(_name);
            if(!excluded.Contains(_name))
            {
                sceneNames.Add(_name);
            }
        }
        return sceneNames;
        
    }
}
