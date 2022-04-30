using UnityEngine;

public class PlayerCollisionScript : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    private float distToGround;

    //--------------------------------------
    //  User Defined Types
    //--------------------------------------
    public Color rayColor;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        distToGround = playerController.collider.bounds.extents.y;
    }

    public bool DetectHasTakenOff()
    {
        bool result;

        if (Physics2D.Raycast(transform.position, Vector2.down, 2.0f))//Colliding with ground
        {
            // Debug.Log("This is team rocket blasting off again!!!!");
            result = false;
        }
        else//Not colliding with the ground
        {
            // Debug.Log("What happened to the engines are they on?");
            result = true;
        }

        rayColor = (result) ? Color.green : Color.red;
        Debug.DrawRay(transform.position, Vector2.down, rayColor);


        return result;
    }

    public bool DetectHasLanded()
    {
        bool result;

        //  This raycast is projecting a Ray downwards and measuring it against distToGround + 0.1f
        //  have we reached a specific from a collider ( Have we move a specific y distance away from the closest collider)
        if (Physics2D.Raycast(transform.position, Vector2.down, distToGround + 0.1f))
        {
            Debug.Log("We crashed and were dead.");
            result = true;
        }
        else//  We've hit the ground
        {
            Debug.Log("One small step for man and a huge leap for mandkind!");
            result = false;
        }

        return result;
    }
}
