using UnityEngine;

public class PlayerInputScript : MonoBehaviour
{
    [SerializeField] PlayerController playerController;

    //--------------------------------------
    //  Inspector: SerializedFields
    //--------------------------------------
    public bool moveLeftInput;
    public bool moveRightInput;

    // Start is called before the first frame update
    void Start()
    {
        moveLeftInput = false;
        moveRightInput = false;
    }

    // Update is called once per frame
    void Update()
    {
        DetectMotion();
    }

    private void DetectMotion()
    {
        if (Input.GetAxis("Horizontal") > 0)
        {
            moveLeftInput = false;
            moveRightInput = true;
        }
        else if (Input.GetAxis("Horizontal") < 0)
        {
            moveLeftInput = true;
            moveRightInput = false;
        }
        else
        {
            moveLeftInput = false;
            moveRightInput = false;
        }

    }

    public bool DetectJumpingEvent()
    {
        if (playerController.playerState == PlayerController.PlayerState_e.Grounded && Input.GetButtonDown("Jump"))
        {
            // Debug.Log("Detecting the player is trying to jump . . .");
            return true;
        }
        return false;
    }

}