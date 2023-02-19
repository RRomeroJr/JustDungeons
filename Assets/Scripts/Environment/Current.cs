using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Current: MonoBehaviour
{
    public float pushStrength = 2.0f;
    void Awake(){

    }
	void OnTriggerStay2D(Collider2D other){
        if(other.tag != "Player"){
            return;
        }
        Bounds bounds = GetComponent<Renderer>().bounds;
        Vector2 pushDirection =  (transform.position + transform.right) - transform.position;
        // pushDirection.Normalize();

        Debug.DrawLine(transform.position, ((Vector2)transform.position + (5.0f *pushDirection)));
        Debug.DrawLine(other.transform.position, (Vector2)other.transform.position + pushDirection);

        other.GetComponent<Actor>().Knockback(pushStrength * pushDirection);
        // other.GetComponent<Rigidbody2D>().AddForce( new Vector2(bounds.))
    }
    
}