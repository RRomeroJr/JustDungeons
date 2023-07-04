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

        switch (state)
        {
            case PlayerState.Alive:
                if(actor.CanMove)
                {
                Vector2 inputVectRaw = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
                // Vector2 inputVect = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
                // Vector2 inputVect = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
                Vector2 combinedInputVect = new Vector2(
                                        Mathf.Clamp(Input.GetAxis("Horizontal") + inputVectRaw.x, -1, 1 ),
                                        Mathf.Clamp(Input.GetAxis("Vertical") + inputVectRaw.y, -1, 1 ));


                if (actor.CanMove && GetComponent<BuffHandler>().Dizzy <= 0)
                {
                    moveDirection = combinedInputVect;
                    
                }

                MoveInDirection(moveDirection.Value);
                }
                MovementFacingDirection();

                break;
            case PlayerState.Dead:
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
                actor.target = TabTargetCycle();
                actor.LocalPlayerBroadcastTarget();
                try{
                    actor.target.nameplate.selectedEvent.Invoke(true);
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
            switch (state)
            {
                case PlayerState.Alive:
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
                case PlayerState.Dead:
                    break;
                default:
                    break;
            }
        }
    }
    void MouseOver()
    {
        RaycastHit2D hit = raycastToActors(HBCTools.GetMousePosWP());
        Actor hitActor = null;
        try{
            hitActor =  hit.collider.transform.parent.GetComponent<Actor>();
        }
        catch{
        }

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

            RaycastHit2D hit = raycastToActors(HBCTools.GetMousePosWP());
            Actor hitActor = null;
            try{
                hitActor =  hit.collider.transform.parent.GetComponent<Actor>();
            }
            catch{
            }
            //Debug.Log("mousePos "+ mousePos.ToString());

            if (hit.collider != null) {
                
                // Debug.Log("Clicked something: " + hit.collider.gameObject.name);
            }else{
                //Debug.Log("Nothing clicked");
                tabIndex = 0;
            }
            // if(HBCTools.areHostle(actor, hitActor) == false){//actor in this case being the player
            //     actor.GetComponent<Controller>().autoAttacking = false;
            // }
            actor.SetTarget(hitActor);
            
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
             


            RaycastHit2D hit = raycastToActors(HBCTools.GetMousePosWP());
            Actor hitActor= null;
            try{
                hitActor =  hit.collider.transform.parent.GetComponent<Actor>();
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
                tabIndex = 0;
            }
            
            actor.SetTarget(hitActor);
        }
        if(Input.GetMouseButton(1)){
            clickTime1 += Time.deltaTime;

            xMouseMove += Input.GetAxis("Mouse X") * mouseSensitivity * -1.0f;

            Vector2 mouseMoveVect;
            //mouseMoveVect = (Vector2)Input.mousePosition - mouseStart0; //mode 1
            mouseMoveVect = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - rendererRef.bounds.BottomCenter(); //mode 2
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
    RaycastHit2D raycastToActors(Vector2 _loc)
    {
        return Physics2D.Raycast(_loc, Vector2.zero, Mathf.Infinity, clickMask);
    }
}
