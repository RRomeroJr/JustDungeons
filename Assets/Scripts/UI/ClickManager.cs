using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
/*
    Richie: 
        This should handle clicking of characters/ objects in the world
    as well as UI elements inorder to interact with them/ get them
    as your current target
*/
public class ClickManager : MonoBehaviour
{
    public Text targetName;
    public Slider targetHealthBar;
    public Image targetHealthBarFill;
    public PlayerControllerHBC playerControler;


    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            /*
                    Implement Clicking UI frames to get target somehow 
            */

            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit.collider != null) {
                //Debug.Log("Clicked something");
                // set controller's target w/ actor hit by raycast
                playerControler.target = hit.collider.gameObject.GetComponent<Actor>();
                /*
                Debug.Log(hit.collider.gameObject.GetComponent<Actor>().name);
                
                targetName.text = hit.collider.gameObject.GetComponent<Actor>().name;
                targetHealthBar.maxValue = hit.collider.gameObject.GetComponent<Actor>().maxHealth;
                targetHealthBar.value = hit.collider.gameObject.GetComponent<Actor>().health;
                targetHealthBarFill.color = hit.collider.gameObject.GetComponent<Actor>().unitColor;*/
                
            }
        }
    }
}
