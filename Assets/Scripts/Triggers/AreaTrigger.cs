using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
/* Activate trigger when player is colliding with trigger.
*/
public class AreaTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {   
        if (other.CompareTag("Player")){
            gameObject.GetComponent<TriggerBase>().isActive = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {   
        if (other.CompareTag("Player")){
            gameObject.GetComponent<TriggerBase>().isActive = false;
        }
    }
}
