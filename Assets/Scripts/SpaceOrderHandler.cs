using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*  
    Attach to character and object in the world in order
    to render them in the correct order
*/
public class SpaceOrderHandler : MonoBehaviour
{
    public Renderer myRenderer;
    private void Start(){
        myRenderer = GetComponent<Renderer>();
    }
    private void LateUpdate()
    {
        myRenderer.sortingOrder = (-((int)(GetComponent<BoxCollider2D>().bounds.min.y * 1000)));
        /*if(Input.GetKeyDown("p")){
            Debug.Log((-((int)(GetComponent<BoxCollider2D>().bounds.min.y * 1000))).ToString());
        }*/
    }
}
