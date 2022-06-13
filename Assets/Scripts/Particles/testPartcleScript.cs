using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testPartcleScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //transform.position = gameObject.parent.transform.position;
        StartCoroutine(killSelfAfterXSecs(1.0f));
    }
    void Update(){
        GetComponent<Renderer>().sortingOrder = gameObject.transform.parent.GetComponent<SpriteRenderer>().sortingOrder + 1;
        //hello?
    }
    IEnumerator killSelfAfterXSecs(float x){
        yield return new WaitForSeconds(x);
        Destroy(gameObject);
    }
}
