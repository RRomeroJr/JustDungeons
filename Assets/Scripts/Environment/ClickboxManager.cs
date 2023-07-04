using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickboxManager : MonoBehaviour
{

    void Awake(){

        GetComponent<BoxCollider2D>().size = transform.parent.GetComponent<Renderer>().bounds.size;
    }

}