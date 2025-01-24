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
    public List<Actor> tabableTargets;
    public Vector2 lastTabVector;
    public Vector2 currentTabVector;
    public int tabIndex = -1;
    public GameObject tabTargetAreaPrefab;
    public GameObject tabTargetObj;
    public Actor hoverActor;
    Renderer rendererRef; // using this to set the point where the character should "rotate" to face mouse
                        //used to be Bounds but I found out that that is a struct. So I can't save a ref to it

    public override bool TryingToMove
    {
        get
        {
            if(Mathf.Abs(new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).magnitude) > 0.0f)
            {
                return true;
            }
            else if(Input.GetMouseButton(0) && Input.GetMouseButton(1))
            {
                return true;
            }
            return base.TryingToMove;
        }
    }
    
    
    
    
    //public Vector2 newVect_;
    protected override void Awake()
    {
        base.Awake();
        moveDirection = Vector2.zero; 
    }
    public override void Start()
    {
        base.Start();
        lastTabVector = Vector2.right;
        if(isLocalPlayer){
            UIManager.playerController = this;
            var uiRaycaster = FindObjectOfType<UIRaycaster>();
            if (uiRaycaster != null)
            {
                uiRaycaster.UIFrameClicked += OnUIFrameClicked;
            }
            rendererRef = GetComponent<Renderer>();
        }
        // tabTargetObj = Instantiate(tabTargetAreaPrefab, HBCTools.GetMousePosWP(), Quaternion.identity);
        // tabTargetObj.GetComponent<TabTargetGetter>().Init(this);

        actor.PlayerIsDead += HandlePlayerDead;
        actor.PlayerIsAlive += HandlePlayerAlive;
    }
    void FixedUpdate()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        switch (actor.state)
        {
            case ActorState.Alive:
                // Debug.Log(gameObject.name + "Alive");
                if(actor.CanMove)
                {

                    if (buffHandler.Dizzy <= 0)
                    {
                        Vector2 inputVectRaw = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
                        // Vector2 inputVect = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
                        // Vector2 inputVect = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
                        // Vector2 combinedInputVect = new Vector2(
                        //                         Mathf.Clamp(Input.GetAxis("Horizontal") + inputVectRaw.x, -1, 1 ),
                        //                         Mathf.Clamp(Input.GetAxis("Vertical") + inputVectRaw.y, -1, 1 ));

                        // combinedInputVect = Vector2.ClampMagnitude(combinedInputVect, 1.0f);
                        inputVectRaw.Normalize();
                        moveDirection = inputVectRaw;
                        if(Input.GetMouseButton(0) && Input.GetMouseButton(1))
                        {
                            Vector2 doubleMouseVect = (Vector2)(HBCTools.GetMousePosWP() - transform.position);
                            doubleMouseVect.Normalize();

                            moveDirection = Vector2.ClampMagnitude(moveDirection.Value + doubleMouseVect, 1.0f);
                        }
                    }
                    MoveInDirection(moveDirection.Value);
                }
                else
                {
                    // Debug.Log(gameObject.name + "Alive can't move");
                }
                // MovementFacingDirection();

                break;
            case ActorState.Dead:
                // Debug.Log(gameObject.name + "Dead");
                break;
            default:
                break;
        }
    }
    // public override void MoveInDirection(Vector2 _direction)
    // {
    //     base.MoveInDirection(_direction);
    //     MovementFacingDirection();
        
    // }
    
    protected override void MovementFacingDirection()
    {
        // Debug.Log("PlayerController MovementFacingDirection");
        if (Input.GetMouseButton(1) == false)
        {
            // Debug.Log("Calling base");
            base.MovementFacingDirection();
        }
    }
    Vector2 PlayerToMouse(){
        return (Vector2)(HBCTools.GetMousePosWP() - transform.position);
    }
    
    /*   Old Code idea
    Actor findTabTargetSmallestAngle(float min, float maxNotInclusive){
        Debug.Log("findTabTargetSmallestAngle( " + min + ", " + maxNotInclusive + ")");
        Actor toReturn = null;
        float smallestAngle = 0.0f;
        foreach(Actor a in tabableTargets){
            float currentAngle = AngleFromPlayerToMouse(a);
            if((min <= currentAngle)&&(currentAngle < maxNotInclusive)){

                if((toReturn == null) || (currentAngle < smallestAngle)){
                    smallestAngle = currentAngle;
                    toReturn = a;
                }

            }
        }
        if(toReturn == null){
            Debug.LogError("Could not find tabable target in angle range (" + min + ", " + maxNotInclusive + ")");
        }
        else{
            Debug.Log("findTabTargetSmallestAngle: " + smallestAngle);
        }
        return toReturn;
    }*/
    
    Actor TabTargetCycle(){

        if(tabIndex + 1 < tabableTargets.Count){
            tabIndex++;
        }
        else{
            tabIndex = 0;
        }
        tabableTargets.Sort(ComparePlayerToMouseAngles);

        
        return tabableTargets[tabIndex];
    }
    int ComparePlayerToMouseAngles(Actor _a1, Actor _a2){
        float _a1Angle = AngleFromPlayerToMouse(_a1);
        float _a2Angle = AngleFromPlayerToMouse(_a2);
  
        
        if(_a1Angle < _a2Angle){
            return -1;
        }
        if(_a1Angle > _a2Angle){
            return 1;
        }
        return 0;

    }
    float AngleFromPlayerToMouse(Actor _target){
        Vector2 playerToMouse = PlayerToMouse();
        Vector3 playerToTarget = _target.transform.position - transform.position;

        return Vector2.Angle((Vector2) playerToTarget, playerToMouse);
    }
    Vector2 GetScreenSizeWU(){
        Vector2 toReturn = new Vector2(Screen.width, Screen.height);
        toReturn.x = (toReturn.x * (Camera.main.orthographicSize * 2.0f))/ toReturn.y;
        toReturn.y = (Camera.main.orthographicSize * 2.0f);
        return toReturn;
    }
    void GetEnemiesOnScreen(){
        RaycastHit2D[] hits;
        Vector2 WUscreenSize = GetScreenSizeWU();
        hits = Physics2D.BoxCastAll(Camera.main.transform.position, WUscreenSize, 0.0f, Vector2.zero, distance: 0.0f, layerMask: LayerMask.GetMask("Enemy"));
        //DrawBox(Camera.main.transform.position, WUscreenSize, Color.blue);
        if(tabableTargets.Count > 0){
            tabableTargets.Clear();
        }
        
        
        if(hits.Length > 0){
            List<Actor> newActorList = new List<Actor>();
            foreach(RaycastHit2D h in hits){
                newActorList.Add(h.collider.GetComponent<Actor>());
            }
            tabableTargets = newActorList;
        }
       
    }
    
    void DrawBox(Vector2 center, Vector2 _size, Color _color){
        Vector2 p1 = center + new Vector2(-_size.x/2.0f, _size.y/2.0f); //Top left
        Vector2 p2 = center + new Vector2(_size.x/2.0f, _size.y/2.0f); //Top right
        Vector2 p3 = center + new Vector2(_size.x/2.0f, -_size.y/2.0f); //Bottom right
        Vector2 p4 = center + new Vector2(-_size.x/2.0f, -_size.y/2.0f); //Bottom left


        Debug.DrawLine(p1, p2, _color);
        Debug.DrawLine(p2, p3, _color);
        Debug.DrawLine(p3, p4, _color);
        Debug.DrawLine(p4, p1, _color);
    }
    
    public override void Update()
    {
        base.Update();
        if (isLocalPlayer)
        {
            // Vector2 debugVect =PlayerToMouse();
            // debugVect.Normalize();
            
             //Debug.DrawLine(transform.position, (5.0f * debugVect) + (Vector2)transform.position, Color.green);
            //-Updating tabble targets
            
            currentTabVector = getWorldPointTarget() - transform.position;
            if(Vector2.Angle(lastTabVector, currentTabVector) > 5.0f){
                lastTabVector = currentTabVector;
                tabIndex = -1;
                GetEnemiesOnScreen();
            }
            //-------------------------------------------
            //-Tab cycle
            if (Input.GetKeyDown("tab")){
                try{
                    actor.target.nameplate.selectedEvent.Invoke(false);
                }
                catch{

                }
                try{
                    actor.target = TabTargetCycle();
                    actor.LocalPlayerBroadcastTarget();
                    actor.target.nameplate.selectedEvent.Invoke(true);
                    if(!Input.GetMouseButton(1) && actor.target) // Face tab target: Make option in future?
                    {
                        var newFace = HBCTools.ToNearest45(actor.target.transform.position - transform.position);
                        facingDirection = newFace;
                        CmdSetFacingDirection(newFace);
                    }
                }
                catch{

                }
            }
            //--------------------------
            //Zoom in out
            // if(Input.GetAxis("Mouse ScrollWheel") > 0.0f){
                
            //     Camera.main.GetComponent<CameraController>().zoomIn();
            // }
            // if(Input.GetAxis("Mouse ScrollWheel") < 0.0f){
                
            //     Camera.main.GetComponent<CameraController>().zoomOut();
            // }
            //-------------------------------------------------
            mouseInput();
            MouseOver();
            switch (actor.state)
            {
                case ActorState.Alive:
                    // if (Input.GetKeyDown("1"))
                    // {
                    //     if (abilities[0] != null)
                    //         actor.castAbility3(abilities[0]);
                    // }
                    // if (Input.GetKeyDown("2"))
                    // {
                    //     if (abilities[1] != null)
                    //         actor.castAbility3(abilities[1]);
                    // }
                    // if (Input.GetKeyDown("3"))
                    // {
                    //     if (abilities[2] != null)
                    //         actor.castAbility3(abilities[2]);
                    // }
                    // if (Input.GetKeyDown("4"))
                    // {
                    //     if (abilities[3] != null)
                    //         actor.castAbility3(abilities[3]);
                    // }
                    // if (Input.GetKeyDown("5"))
                    // {
                    //     if (abilities[4] != null)
                    //         actor.castAbility3(abilities[4]);
                    // }
                    // if (Input.GetKeyDown("q"))
                    // {
                    //     if (abilities[5] != null)
                    //         actor.castAbility3(abilities[5]);
                    // }
                    // if (Input.GetKeyDown("e"))
                    // {
                    //     if (abilities[6] != null)
                    //         actor.castAbility3(abilities[6]);
                    // }
                    // if (Input.GetKeyDown("r"))
                    // {
                    //     if (abilities[7] != null)
                    //         actor.castAbility3(abilities[7]);
                    // }
                    // if (Input.GetKeyDown("f"))
                    // {
                    //     if (abilities[8] != null)
                    //         actor.castAbility3(abilities[8]);
                    // }
                    break;
                case ActorState.Dead:
                    break;
                default:
                    break;
            }
        }
    }
    void MouseOver()
    {
        Actor hitActor = Targeting.LookForNewTarget();

        if(hitActor == hoverActor)
        {
            return;
        }

        if(hoverActor != null){
            hoverActor.OnHoverEnd();
        }
        
        hoverActor = hitActor;

        if(hoverActor != null){
            hoverActor.OnHoverStart();
        }

        
    }
    void mouseInput(){
        if ((UIManager.Instance.MouseButtonClick(0) && !Input.GetMouseButton(1)) || (UIManager.Instance.MouseButtonClick(1) && !Input.GetMouseButton(0)))
        {

            Actor hitActor = Targeting.LookForNewTarget();

            if (hitActor == null ) // No target was found
            {
                tabIndex = 0;
                autoAttacking = false;
            }
            else if (Input.GetMouseButtonUp(1)) //If a target found and was right-clicked
            {
                if (HBCTools.areHostle(transform, hitActor.transform))
                {
                    autoAttacking = true;
                    if(Vector2.Distance(transform.position, hitActor.transform.position) > Ability_V2.meleeRange)
                    {
                        MsgBox.NotInRange();
                    }
                }
                else
                {
                    autoAttacking = false;
                }
            }

            actor.SetTarget(hitActor);
            
        }
        if(Input.GetMouseButton(1) && buffHandler.Dizzy <= 0){

            facingDirection = HBCTools.ToNearest45(Camera.main.ScreenToWorldPoint(Input.mousePosition) - rendererRef.bounds.center);
            // facingDirection = HBCTools.ToNearest45((Vector2)(Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position));
            CmdSetFacingDirection(facingDirection);
            
        }
    }

    void HandlePlayerDead(object sender, EventArgs e)
    {
        actor.state = ActorState.Dead;
        TempSpriteManager sprite = GetComponent<TempSpriteManager>();
        transform.Rotate(0f, 0f, 90.0f);
        sprite.enabled = false; // temp fix for being able to move sprite while dead
    }
    void HandlePlayerAlive(object sender, EventArgs e)
    {
        actor.state = ActorState.Alive;
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
