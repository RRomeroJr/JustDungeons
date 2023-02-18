using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.AI;
using System;

public enum PlayerState
{
    Alive,
    Dead
}

public class PlayerController : Controller
{
   
    public PlayerState state;
    public LayerMask clickMask;
    public float mouseSensitivity = 15f;
    public float xMouseMove = 0.0f;
    public float clickTime0 = 0.0f;
    public float clickTime1 = 0.0f;
    public Vector2 mouseStart0;
    public Vector2 mouseStart1;
    public Vector2 mousePos;
    public float clickWindow = 0.66f;
    public float clickTravelWindow = 66.0f;
    
    //public Vector2 newVect_;
    void Awake(){
        actor = GetComponent<Actor>();
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }
    public override void Start()
    {   
        base.Start();
        if(isLocalPlayer){
            UIManager.playerController = this;
            FindObjectOfType<UIRaycaster>().UIFrameClicked += OnUIFrameClicked;
        }

        actor.PlayerIsDead += HandlePlayerDead;
        actor.PlayerIsAlive += HandlePlayerAlive;
    }
    void FixedUpdate(){
        if (isLocalPlayer){
            
            switch (state)
            {
                case PlayerState.Alive:
                    if (actor.canMove)
                    {
                        Vector2 newVect = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
                        
                        if (Mathf.Abs(newVect.magnitude) > 0.2)
                        {
                            if(!tryingToMove){
                                CmdSetTryingToMove(true);
                            }
                            
                            MoveInDirection(newVect);

                            if(Input.GetMouseButton(1) == false){
                                HBCTools.Quadrant newVectQuad;
                                newVectQuad = HBCTools.GetQuadrant(newVect);
                                if((newVect.x == 0.0f)||(newVect.y == 0.0f)){ //don't try to chan
                                    break;
                                }
                                if(HBCTools.GetQuadrant(facingDirection) != newVectQuad){
                                    facingDirection = HBCTools.QuadrantToVector(newVectQuad);
                                    CmdSetFacingDirection(facingDirection);
                                }
                            }
 
                        }
                        else{
                            if(tryingToMove){
                                CmdSetTryingToMove(false);
                            }
                        }
                        //if(not in deadzone)
                        // Vector2 newVect = new Vector2(Input.GetAxis("Horizontal") * HORIZ_MOVE_ACCEL * Time.deltaTime,
                        //     Input.GetAxis("Vertical") * VERT_MOVE_ACCEL * Time.deltaTime);
                        // gameObject.GetComponent<Rigidbody2D>().velocity = newVect;

                    }
                    break;
                case PlayerState.Dead:
                    break;
                default:
                    break;
            }
        }
    }

    public override void Update()
    {
        base.Update();
        if (isLocalPlayer)
        {
            mouseInput();
            switch (state)
            {
                case PlayerState.Alive:
                    if (Input.GetKeyDown("1"))
                    {
                        if (abilities[0] != null)
                            actor.castAbility3(abilities[0]);
                    }
                    if (Input.GetKeyDown("2"))
                    {
                        if (abilities[1] != null)
                            actor.castAbility3(abilities[1]);
                    }
                    if (Input.GetKeyDown("3"))
                    {
                        if (abilities[2] != null)
                            actor.castAbility3(abilities[2]);
                    }
                    if (Input.GetKeyDown("4"))
                    {
                        if (abilities[3] != null)
                            actor.castAbility3(abilities[3]);
                    }
                    if (Input.GetKeyDown("5"))
                    {
                        if (abilities[4] != null)
                            actor.castAbility3(abilities[4]);
                    }
                    if (Input.GetKeyDown("q"))
                    {
                        if (abilities[5] != null)
                            actor.castAbility3(abilities[5]);
                    }
                    if (Input.GetKeyDown("e"))
                    {
                        if (abilities[6] != null)
                            actor.castAbility3(abilities[6]);
                    }
                    if (Input.GetKeyDown("r"))
                    {
                        if (abilities[7] != null)
                            actor.castAbility3(abilities[7]);
                    }
                    if (Input.GetKeyDown("f"))
                    {
                        if (abilities[8] != null)
                            actor.castAbility3(abilities[8]);
                    }
                    break;
                case PlayerState.Dead:
                    break;
                default:
                    break;
            }
        }
    }
    void mouseInput(){
        if (Input.GetMouseButtonDown(0)) {
            clickTime0 = 0.0f;
            mouseStart0 = Input.mousePosition;
            
        }
        if (Input.GetMouseButtonUp(0)) {
            float mouseTravel = Vector2.Distance(mouseStart0, Input.mousePosition);
            if((clickTime0 > clickWindow) || (mouseTravel > clickTravelWindow)){
                clickTime0 = 0.0f;
                return;
            }
            clickTime0 = 0.0f;
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, Mathf.Infinity, clickMask);
            Actor hitActor = null;
            try{
                hitActor =  hit.collider.gameObject.GetComponent<Actor>();
            }
            catch{
            }
            //Debug.Log("mousePos "+ mousePos.ToString());

            if (hit.collider != null) {
                
                Debug.Log("Clicked something: " + hit.collider.gameObject.name);
            }else{
                //Debug.Log("Nothing clicked");
                
            }
            // if(HBCTools.areHostle(actor, hitActor) == false){//actor in this case being the player
            //     actor.GetComponent<Controller>().autoAttacking = false;
            // }
            actor.target = hitActor;
            actor.LocalPlayerBroadcastTarget();
        }
        if(Input.GetMouseButton(0)){
            clickTime0 += Time.deltaTime;
        }
        if (Input.GetMouseButtonDown(1)) {
            clickTime1 = 0.0f;
            mouseStart1 = Input.mousePosition; //used for manual turning
            
        }
        
        if (Input.GetMouseButtonUp(1)) {
            float mouseTravel = Vector2.Distance(mouseStart1, Input.mousePosition);
            //Debug.Log(mouseTravel.ToString());
            if((clickTime1 > clickWindow) || (mouseTravel > clickTravelWindow)){
                clickTime1 = 0.0f;
                return;
            }

            clickTime1 = 0.0f;
             Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
             


            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, Mathf.Infinity, clickMask);
            Actor hitActor= null;
            try{
                hitActor =  hit.collider.gameObject.GetComponent<Actor>();
            }
            catch{
            }
            //Debug.Log("mousePos "+ mousePos.ToString());

            if (hit.collider != null) {
                
                Debug.Log("Clicked something: " + hit.collider.gameObject.name);
                // set controller's target w/ actor hit by raycast
                
                if(HBCTools.areHostle(actor, hitActor)){//actor in this case being the player
                    actor.GetComponent<Controller>().autoAttacking = true;
                }
                else{
                    actor.GetComponent<Controller>().autoAttacking = false;
                }
                
                
            }else{
                //Debug.Log("Nothing clicked");
                actor.GetComponent<Controller>().autoAttacking = false;
            }
            
            actor.target = hitActor;
            actor.LocalPlayerBroadcastTarget();
        }
        if(Input.GetMouseButton(1)){
            clickTime1 += Time.deltaTime;

            xMouseMove += Input.GetAxis("Mouse X") * mouseSensitivity * -1.0f;

            Vector2 mouseMoveVect;
            //mouseMoveVect = (Vector2)Input.mousePosition - mouseStart0; //mode 1
            mouseMoveVect = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - (Vector2)transform.position; //mode 2
            facingDirection = HBCTools.ToNearest45(mouseMoveVect);
            CmdSetFacingDirection(facingDirection);
            
        }
    }

    void HandlePlayerDead(object sender, EventArgs e)
    {
        state = PlayerState.Dead;
        TempSpriteManager sprite = GetComponent<TempSpriteManager>();
        transform.Rotate(0f, 0f, 90.0f);
        sprite.enabled = false; // temp fix for being able to move sprite while dead
    }
    void HandlePlayerAlive(object sender, EventArgs e)
    {
        state = PlayerState.Alive;
        TempSpriteManager sprite = GetComponent<TempSpriteManager>();
        transform.rotation = Quaternion.identity;
        sprite.enabled = true; // temp fix for being able to move sprite while dead
    }

    [Command]
    void reqAbilityEff(AbilityEff eff){
        Debug.Log("A client reqed an AbilityEff " + eff.effectName);
    }
    
    public Vector3 getWorldPointTarget(){
        Vector3 scrnPos = Input.mousePosition;
        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(scrnPos);
        worldPoint.z = 0.0f;
        return worldPoint;
    }
    public Vector3 getRelWPTarget(){
        Vector3 scrnPos = Input.mousePosition;
        Vector3 relWP = Camera.main.ScreenToWorldPoint(scrnPos);
        relWP.z = 0.0f;
        relWP = relWP - transform.position;
        return relWP;
    }
    private void OnUIFrameClicked(object source, UIFrameClickedEventArgs e)
    {
        if (isServer)
        {
            actor.rpcSetTarget(e.Frame.actor);
        }
        else
        {
            actor.cmdReqSetTarget(e.Frame.actor);
        }
    }

    [ClientRpc]
    void updateTargetToClients(Actor target){
        actor.target = target;
    }
    [Command]
    void reqTargetUpdate(Actor _actor){ //in future this should be some sort of act id or something
        updateTargetToClients(_actor);
    }
}
