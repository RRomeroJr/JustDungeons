using UnityEngine;

public class PlayerController : MonoBehaviour
{
    const float IN_FLIGHT_X_MOVEMENT_MODIFIER = 350;
    const float FALL_RATE = 3.0f;
    const float MAX_JUMP_SPEED = 3.0f;
    const float MAX_RUN_SPEED = 3.0f;

    private Rigidbody2D body;
    private Collider2D collider;

    [SerializeField] public float MOVE_ACCELERATION = 360;
    [SerializeField] public float JUMP_ACCELERATION = 300;

    public PlayerState_e playerState = PlayerState_e.Grounded;
    public float distToGround;

    public bool jump;

    public Color rayColor;

    public enum PlayerState_e
    {
        Grounded,
        PreparingToJump,
        InFlight,
        //PreparingToSlide,
        //Sliding,
    };

    // Start is called before the first frame update
    private void Awake()
    {
        playerState = PlayerState_e.Grounded;
        body = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
    }

    private void Start()
    {
        distToGround = collider.bounds.extents.y;
    }

    //  Player input detection goes into Update()
    private void Update()
    {
        UpdateStateMachine();
    }

    //  All PHYSICS go in FixedUpdate()
    private void FixedUpdate()
    {
        ComputeVelocity();
    }

    void UpdateStateMachine()
    //  The state machine will be the main driver of all mechanism related to the movement of the character
    {

        switch (playerState)
        {
            case PlayerState_e.Grounded:
                if (DetectJumpingEvent())
                {
                    Debug.Log("Grounded -> PreparingToJump");
                    playerState = PlayerState_e.PreparingToJump;
                }
                else
                {
                    playerState = PlayerState_e.Grounded;
                }
                break;

            case PlayerState_e.PreparingToJump:
                //  Setup all parameters to take flight
                if (DetectHasTakenOff())
                {
                    Debug.Log("PreparingToJump -> InFlight");
                    playerState = PlayerState_e.InFlight;
                }
                break;

            case PlayerState_e.InFlight:
                //  Continuously check if we have hit the ground yet.
                //  and apply all modifiers to get "normal" kinematic movement
                if (DetectHasLanded())
                {
                    Debug.Log("InFlight -> Grounded");
                    playerState = PlayerState_e.Grounded;
                }
                else
                {
                    playerState = PlayerState_e.InFlight;
                }
                break;

            default:
                playerState = PlayerState_e.Grounded;
                break;

        }
    }

    bool DetectJumpingEvent()
    {
        if (playerState == PlayerState_e.Grounded && Input.GetButtonUp("Jump"))
        {
            Debug.Log("Detecting the player is trying to jump . . .");
            return true;
        }
        return false;
    }

    bool DetectHasTakenOff()
    {
        bool result;
        RaycastHit2D test_raycast;
        test_raycast = Physics2D.Raycast(transform.position, Vector2.down, distToGround + 30.0f);

        if (Physics2D.Raycast(transform.position, -Vector2.up, 2.0f))//Colliding with ground
        {
            Debug.Log("This is team rocket blasting off again!!!!");
            result = false;
        }
        else//Not colliding with the ground
        {
            Debug.Log("What happened to the engines are they on?");
            result = true;
        }

        // Vector2 test_vec = new Vector2(0.0f, distToGround + 0.1f);
        // test_vec = test_vec * Vector2.down;

        rayColor = (result) ? Color.green : Color.red;
        Debug.DrawRay(transform.position, Vector2.down, rayColor);

        return result;
    }
    bool DetectHasLanded()
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

    void ComputeVelocity()
    //  Based upon the current state of the Player calculate their their velocity
    {
        //  We're currently on the ground one x-direction movement should change
        if (playerState == PlayerState_e.Grounded)
        {
            body.velocity = new Vector2(Input.GetAxis("Horizontal") * MOVE_ACCELERATION * Time.deltaTime, body.velocity.y);
        }
        else if (playerState == PlayerState_e.PreparingToJump)
        {
            Vector2 movement;

            //  Capture x velocity right before we jump
            movement.x = Input.GetAxis("Horizontal") * MOVE_ACCELERATION * Time.deltaTime;
            movement.y = 0;
            movement.y += JUMP_ACCELERATION * Time.deltaTime;

            body.velocity = movement;

            /*
                Richie's new school method for jumping and it works!
                Keep this in our back pocket
            */

            // Vector2 jump_vector = new Vector2(0.0f, 500.0f);
            // body.AddForce(jump_vector);

        }
        else if (playerState == PlayerState_e.InFlight)
        {
            body.velocity = new Vector2(Input.GetAxis("Horizontal") * IN_FLIGHT_X_MOVEMENT_MODIFIER * Time.deltaTime, body.velocity.y);
        }

    }


}
