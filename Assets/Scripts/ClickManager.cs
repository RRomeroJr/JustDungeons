using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ClickManager : MonoBehaviour
{
    public Text targetName;
    public Slider targetHealthBar;
    public Image targetHealthBarFill;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            /*
                    Implement Clicking UI frames to get target somehow 
            */

            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit.collider != null) {
                Debug.Log("Clicked something");
                Debug.Log(hit.collider.gameObject.GetComponent<Actor>().name);
                
                targetName.text = hit.collider.gameObject.GetComponent<Actor>().name;
                targetHealthBar.maxValue = hit.collider.gameObject.GetComponent<Actor>().maxHealth;
                targetHealthBar.value = hit.collider.gameObject.GetComponent<Actor>().health;
                targetHealthBarFill.color = hit.collider.gameObject.GetComponent<Actor>().unitColor;
                
            }
        }
    }
}
