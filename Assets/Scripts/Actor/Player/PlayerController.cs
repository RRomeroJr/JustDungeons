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
            actor.PlayerIsDead += HandlePlayerDead;
        }

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
                            newVect.x *= (moveSpeed * Time.deltaTime);
                            newVect.y *= (moveSpeed * Time.deltaTime);
                            gameObject.GetComponent<Rigidbody2D>().velocity = newVect;
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

    void HandlePlayerDead(object sender, EventArgs e)
    {
        actor.PlayerIsDead -= HandlePlayerDead;
        actor.PlayerIsAlive += HandlePlayerAlive;
        state = PlayerState.Dead;
        TempSpriteManager sprite = GetComponent<TempSpriteManager>();
        sprite.transform.Rotate(0f, 0f, 90.0f);
        sprite.enabled = false; // temp fix for being able to move sprite while dead
    }
    void HandlePlayerAlive(object sender, EventArgs e)
    {
        actor.PlayerIsDead -= HandlePlayerAlive;
        actor.PlayerIsDead += HandlePlayerDead;
        state = PlayerState.Alive;
        TempSpriteManager sprite = GetComponent<TempSpriteManager>();
        sprite.transform.rotation = Quaternion.identity;
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
}
