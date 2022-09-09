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
    public Actor playerActor;


    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            /*
                    Implement Clicking UI frames to get target somehow 
            */

            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //Debug.Log("mousePos " + mousePos.ToString());
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit.collider != null) {
                //Debug.Log("Clicked something");

                // set controller's target w/ actor hit by raycast
                playerActor.target = hit.collider.gameObject.GetComponent<Actor>();
                
            }
        }
    }
}
