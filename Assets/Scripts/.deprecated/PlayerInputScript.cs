using UnityEngine;

public class PlayerInputScript : MonoBehaviour
{
    [SerializeField] private PlayerControllerOLD PlayerControllerOLD;

    //--------------------------------------
    //  Inspector: SerializedFields
    //--------------------------------------
    public bool moveLeftInput;
    public bool moveRightInput;
    public bool isFiring;

    // Start is called before the first frame update
    private void Start()
    {
        moveLeftInput = false;
        moveRightInput = true;  //  TODO: This is not great, are we always going to spawn the character facing right?
        isFiring = false;
    }

    // Update is called once per frame
    private void Update()
    {
        DetectMotion();
        DetectFiringInput();
    }

    private void DetectMotion()
    {
        if (Input.GetAxis("Horizontal") > 0)
        {
            //  If we were previously moving left, flip the character to the left
            if (moveLeftInput)
            {
                PlayerControllerOLD.transform.Rotate(0f, 180f, 0f);
            }

            moveLeftInput = false;
            moveRightInput = true;
        }
        else if (Input.GetAxis("Horizontal") < 0)
        {
            //  If we were previously moving right, flip the character to the left
            if (moveRightInput)
            {
                PlayerControllerOLD.transform.Rotate(0f, 180f, 0f);
            }

            moveLeftInput = true;
            moveRightInput = false;
        }
    }

    public bool DetectFiringInput()
    {
        if (!Input.GetButtonDown("Fire1"))
        {
            return false;
        }

        Debug.Log("Detected Pew pew pew . . .");
        return true;

    }

    public bool DetectJumpingEvent()
    {
        if (PlayerControllerOLD.playerState == PlayerControllerOLD.PlayerState_e.Grounded && Input.GetButtonDown("Jump"))
        {
            // Debug.Log("Detecting the player is trying to jump . . .");
            return true;
        }

        return false;
    }
}
