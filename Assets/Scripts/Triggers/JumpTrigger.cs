using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/* Activate trigger when jump button is held.
*/
public class JumpTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("space")){
            gameObject.GetComponent<TriggerBase>().isActive = true;
        }
        if(Input.GetKeyUp("space")){
            gameObject.GetComponent<TriggerBase>().isActive = false;
        }
    }
}
