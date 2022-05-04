using UnityEngine;

public class PlayerMovementScript : MonoBehaviour
{
    //--------------------------------------
    //  CONSTANTS
    //--------------------------------------
    [SerializeField] private float IN_FLIGHT_X_MOVEMENT_MODIFIER = 350;
    [SerializeField] private float FALL_RATE = 1.1f;
    [SerializeField] private float MAX_JUMP_SPEED = 3.0f;
    [SerializeField] private float MAX_RUN_SPEED = 3.0f;

    //--------------------------------------
    //  Inspector: SerializedFields
    //--------------------------------------
    [SerializeField] private float HORIZ_MOVE_ACCEL = 360;
    [SerializeField] private float VERT_MOVE_ACCEL = 300;

    [SerializeField] private PlayerController playerController;

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
        //  Based upon the current state of the Player calculate their their velocity

        //  We're currently on the ground one x-direction movement should change
        if (playerController.playerState == PlayerController.PlayerState_e.Grounded)
        {
            playerController.body.velocity = new Vector2(
                Input.GetAxis("Horizontal") * HORIZ_MOVE_ACCEL * Time.deltaTime,
                playerController.body.velocity.y
            );

            // playerController.body.AddForce(
            //     new Vector2(
            //         Input.GetAxis("Horizontal") * HORIZ_MOVE_ACCEL * Time.deltaTime,
            //         playerController.body.velocity.y
            //     ),
            //     ForceMode2D.Impulse
            // );
        }
        else if (playerController.playerState == PlayerController.PlayerState_e.PreparingToJump)
        {
            Vector2 movement;

            //  Capture x velocity right before we jump
            movement.x = Input.GetAxis("Horizontal") * HORIZ_MOVE_ACCEL * Time.deltaTime;
            movement.y = VERT_MOVE_ACCEL * Time.deltaTime;

            playerController.body.velocity = movement;

            /*
                Richie's new school method for jumping and it works!
                Keep this in our back pocket
            */

            // playerController.body.AddForce(
            //     new Vector2(
            //         Input.GetAxis("Horizontal") * HORIZ_MOVE_ACCEL * Time.deltaTime,
            //         VERT_MOVE_ACCEL * Time.deltaTime
            //     ),
            //     ForceMode2D.Impulse
            // );

        }
        else if (playerController.playerState == PlayerController.PlayerState_e.InFlight)
        {

            Vector2 inFlightVelocity = playerController.body.velocity;

            //  Add fast fall rate
            inFlightVelocity.y -= FALL_RATE * Time.deltaTime;

            //  Allow player to continue to move inFlight instead of using the 
            //  same tradjectory for the entire jumping action
            inFlightVelocity.x = Input.GetAxis("Horizontal") * HORIZ_MOVE_ACCEL * Time.deltaTime;

            playerController.body.velocity = inFlightVelocity;

        }

    }
}
