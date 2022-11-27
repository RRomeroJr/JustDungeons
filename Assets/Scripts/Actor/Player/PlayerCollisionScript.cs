using UnityEngine;

public class PlayerCollisionScript : MonoBehaviour
{
    [SerializeField] PlayerController playerController;

    private float distToGround;
    public bool isGrounded;
    private Vector3 bodyCenter;
    private float bodyWidth;
    private float bodyHeight;
    private Vector2 boxCastSize;
    private RaycastHit2D platformDetection;

    //--------------------------------------
    //  User Defined Types
    //--------------------------------------
    public Color rayColor;

    // Start is called before the first frame update
    void Start()
    {
        bodyWidth = playerController.collider.bounds.size.x;
        bodyHeight = playerController.collider.bounds.size.y;

        boxCastSize = new Vector2(
            bodyWidth,
            bodyHeight / 6
        );
    }

    // Update is called once per frame
    void Update()
    {
        distToGround = playerController.collider.bounds.extents.y;

        bodyCenter = transform.position;

        platformDetection = Physics2D.BoxCast(
            bodyCenter,     // Origin
            boxCastSize,    // Size
            0,              // Angle
            Vector2.down,   // Direction
            bodyHeight / 2  // Distance
        );


        //  Continuously cast the collision box to see if
        //  if the player has come in contact with another collider

        //
        rayColor = isGrounded ? Color.red : Color.green;

        //  Drawing the right most line of the Physics2D.BoxCast
        Debug.DrawRay(
            new Vector3(
              bodyCenter.x - (bodyWidth / 2),
              bodyCenter.y - (bodyHeight / 2)
            ),
            Vector2.down * bodyHeight / 6,
            rayColor
        );

        //  Drawing the left most line of the Physics2D.BoxCast
        Debug.DrawRay(
            new Vector3(
              bodyCenter.x + (bodyWidth / 2),
              bodyCenter.y - (bodyHeight / 2)
            ),
            Vector2.down * bodyHeight / 6,
            rayColor
        );

        //  Drawing the bottom most line of the Physics2D.BoxCast
        Debug.DrawLine(
            new Vector3(
              bodyCenter.x + (bodyWidth / 2),
              bodyCenter.y - (bodyHeight / 2) - (bodyHeight / 6)
            ),
            new Vector3(
              bodyCenter.x - (bodyWidth / 2),
              bodyCenter.y - (bodyHeight / 2) - (bodyHeight / 6)
            ),
            rayColor
        );

    }

    public bool DetectHasTakenOff()
    {
        bool result;

        RaycastHit2D platformDetection = Physics2D.BoxCast(
            transform.position, // Origin
            boxCastSize,        // Size
            0,                  // Angle
            Vector2.down,       // Direction
            bodyHeight / 2      // Distance
        );

        if (platformDetection)  //Colliding with ground
        {
            // Debug.Log("This is team rocket blasting off again!!!!");
            result = false;
        }
        else//Not colliding with the ground
        {
            // Debug.Log("What happened to the engines are they on?");
            result = true;
        }

        rayColor = result ? Color.green : Color.red;
        Debug.DrawRay(transform.position, Vector2.down, rayColor);


        return result;
    }

    public bool DetectHasLanded()
    {
        bool result;

        //  This raycast is projecting a Ray downwards and measuring it against distToGround + 0.1f
        //  have we reached a specific from a collider ( Have we move a specific y distance away from the closest collider)
        /* if (Physics2D.Raycast(transform.position, Vector2.down, distToGround + 0.1f)) */
        if (platformDetection)
        {
            // Debug.Log("We crashed and were dead.");
            result = true;
            isGrounded = true;

        }
        else//  We've hit the ground
        {
            // Debug.Log("One small step for man and a huge leap for mandkind!");
            result = false;
            isGrounded = false;
        }

        return result;
    }
}
