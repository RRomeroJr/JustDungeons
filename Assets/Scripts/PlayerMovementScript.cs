using UnityEngine;

public class PlayerMovementScript : MonoBehaviour
{
    //--------------------------------------
    //  CONSTANTS
    //--------------------------------------
    const float IN_FLIGHT_X_MOVEMENT_MODIFIER = 350;
    const float FALL_RATE = 3.0f;
    const float MAX_JUMP_SPEED = 3.0f;
    const float MAX_RUN_SPEED = 3.0f;

    //--------------------------------------
    //  Inspector: SerializedFields
    //--------------------------------------
    [SerializeField] public float HORIZ_MOVE_ACCEL = 360;
    [SerializeField] public float VERT_MOVE_ACCEL = 300;

    [SerializeField] PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ComputeVelocity()
    {
        Debug.Log("playerController asking movementScript, are we moving?");
        //  Based upon the current state of the Player calculate their their velocity

        //  We're currently on the ground one x-direction movement should change
        if (playerController.playerState == PlayerController.PlayerState_e.Grounded)
        {
            Debug.Log("Did I get in here?");
            playerController.body.velocity = new Vector2(Input.GetAxis("Horizontal") * HORIZ_MOVE_ACCEL * Time.deltaTime, playerController.body.velocity.y);
        }
        else if (playerController.playerState == PlayerController.PlayerState_e.PreparingToJump)
        {
            Vector2 movement;

            //  Capture x velocity right before we jump
            movement.x = Input.GetAxis("Horizontal") * HORIZ_MOVE_ACCEL * Time.deltaTime;
            // movement.y = 0;
            // movement.y += JUMP_ACCELERATION * Time.deltaTime;
            movement.y = VERT_MOVE_ACCEL * Time.deltaTime;

            playerController.body.velocity = movement;

            /*
                Richie's new school method for jumping and it works!
                Keep this in our back pocket
            */

            // Vector2 jump_vector = new Vector2(0.0f, 500.0f);
            // body.AddForce(jump_vector);

        }
        else if (playerController.playerState == PlayerController.PlayerState_e.InFlight)
        {
            playerController.body.velocity = new Vector2(Input.GetAxis("Horizontal") * IN_FLIGHT_X_MOVEMENT_MODIFIER * Time.deltaTime, playerController.body.velocity.y);
        }

    }
}
