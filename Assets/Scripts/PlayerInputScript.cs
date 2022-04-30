using UnityEngine;

public class PlayerInputScript : MonoBehaviour
{
    [SerializeField] PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool DetectJumpingEvent()
    {
        Debug.Log("PlayerController asking inputScript are we jumping?");

        if (playerController.playerState == PlayerController.PlayerState_e.Grounded && Input.GetButtonDown("Jump"))
        {
            // Debug.Log("Detecting the player is trying to jump . . .");
            return true;
        }
        return false;
    }

}