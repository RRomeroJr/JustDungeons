using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DungeonInfo : MonoBehaviour
{
    // Use Uimanager to reference the player
    // interate over player's cooldowns
    // display them in the textmeshpro.SetText
    public TextMeshProUGUI tmpGUI;
    

    // Start is called before the first frame update
    void Start()
    {
        tmpGUI = GetComponent<TextMeshProUGUI>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
        string toDisplay = "Time: ";
        toDisplay = toDisplay + GameManager.instance.timer.ToString();
        toDisplay = toDisplay + "\nMob Count: " + GameManager.instance.mobCount.ToString();
        tmpGUI.SetText(toDisplay);
        
    }
    
    
}
