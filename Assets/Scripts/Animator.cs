using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animator : MonoBehaviour
{
    public GameObject testParticlesPrefab;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // if damage taken
        // do stuff
        
    }
    public void damage(){
        if(testParticlesPrefab != null)
            Instantiate(testParticlesPrefab, gameObject.transform);
    }
}
