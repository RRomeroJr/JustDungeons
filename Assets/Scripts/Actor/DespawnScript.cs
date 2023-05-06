using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DespawnScript: MonoBehaviour
{
	public float despawnTimer = 10.0f;
    public Actor actor;

    void Awake()
    {
        actor = GetComponent<Actor>();
        if(actor == null)
        {
            Debug.LogError(gameObject.name + "." + GetType() + ": No actor found Destroying DespawnScript");
            Destroy(this);
        }
        else{
            Debug.Log(gameObject.name + "." + GetType() + ": Spawning in " + despawnTimer);
        }
    }
    void Update()
    {
        
        despawnTimer -= Time.deltaTime;
        if(despawnTimer <= 0 ){
            Destroy(gameObject);
        }
    }
}