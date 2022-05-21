using UnityEngine;
using UnityEngine.UI;
/*
    Control for player movement. 
    
    *NOTE* for now there are things related to player health in here.
    This will be changed later 
*/
public class PlayerMovementScript2 : MonoBehaviour
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
    [SerializeField] private float VERT_MOVE_ACCEL = 360;

    [SerializeField] private PlayerController playerController;
    //--------------------------------------
    // vvv This should not be here vvv
    //--------------------------------------
    /*
    public float health;
    public Slider healthSlider;
    

    public void setHealth(float newHealth){
        healthSlider.value = newHealth;
    }
    public void takeDamage(float amount){
        healthSlider.value = healthSlider.value - amount;
    }*/
    //--------------------------------------
    // ^^^ move in future ^^^
    //--------------------------------------
    void Start()
    {

    }
    void Update(){
       
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 newVect = new Vector2(Input.GetAxis("Horizontal") * HORIZ_MOVE_ACCEL * Time.deltaTime,
                Input.GetAxis("Vertical") * VERT_MOVE_ACCEL * Time.deltaTime);

        gameObject.GetComponent<Rigidbody2D>().velocity = newVect;

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

        }
        
    }
}
