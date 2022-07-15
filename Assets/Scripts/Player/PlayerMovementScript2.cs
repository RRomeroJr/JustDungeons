using UnityEngine;
using UnityEngine.UI;
/*
    Control for player movement. 
    
    *NOTE* for now there are things related to player health in here.
    This will be changed later 
*/
public class PlayerMovementScript2 : MonoBehaviour{
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

    [SerializeField] private AbilityEffect lastMovementEffect;
    //  Checking if this ^ was null to controll movment was really buggy.
    //  for some reason if (__ == null) would be true after a = null
    [SerializeField] private bool dashing = false;
    //public bool canMove = true;
    
    void Start(){
        
    }
    void Update(){
       
    }

    // Update is called once per frame
    void FixedUpdate(){
        //Debug.Log(getDashing().ToString());
        if(dashing == false){
            Vector2 newVect = new Vector2(Input.GetAxis("Horizontal") * HORIZ_MOVE_ACCEL * Time.deltaTime,
                Input.GetAxis("Vertical") * VERT_MOVE_ACCEL * Time.deltaTime);
            gameObject.GetComponent<Rigidbody2D>().velocity = newVect;
        }
        else{
            
            transform.position = Vector2.MoveTowards(transform.position, lastMovementEffect.getTargetWP(), lastMovementEffect.getPower());
            if(transform.position == lastMovementEffect.getTargetWP()){
                lastMovementEffect = null;
                dashing = false;
            }
            
        }
        //Debug.Log(lastMovementEffect.getEffectName());
        
    }
    public AbilityEffect getLastMovementEffect(){
        return lastMovementEffect;
    }
    public void setLastMovementEffect(AbilityEffect _lastMovementEffect){
    lastMovementEffect  = _lastMovementEffect;
    }

    public bool getDashing(){
        return dashing;
    }
    public void setDashing(bool _dashing){
        dashing = _dashing;
    }
    
}
