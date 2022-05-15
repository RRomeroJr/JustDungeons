using UnityEngine;

/*
    ----------------------------***PURPOSE***--------------------------------------
    Richie:
        Tried to make a version of the projectile script that makes 
        the projectiles move without using any rigid body stuff

        in order to have projectiles that didn't interact with 
        the physics of the environment or chars

        This may not be need anymore, and
        if this doesn't work out, theoretically it could be swapped out for
        the old script
*/
public class ProjectilePrefabScript2 : MonoBehaviour
{
    public float speed = 0.01f;
    // Awake is upon object construction
    private void Awake()
    {
        speed = 0.01f;
    }
    private void Update()
    {
        if(gameObject.transform.rotation.y >= 0.0f){//  Every update move [speed] amount in the x direction the projectile is facing
            gameObject.transform.position = new Vector2(gameObject.transform.position.x + speed, gameObject.transform.position.y);
        }
        else{
            gameObject.transform.position = new Vector2(gameObject.transform.position.x - speed, gameObject.transform.position.y);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log($"CollisionEvent with {collision.gameObject.tag:s}");
    }

}
