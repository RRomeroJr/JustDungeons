using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.AI;

public enum PlayerState
{
    Alive,
    Dead
}
    
public class PlayerControllerHBC : Controller
{
    [SerializeField] private float HORIZ_MOVE_ACCEL = 360;
    [SerializeField] private float VERT_MOVE_ACCEL = 360;
    public UIManager uiManager;
    Ability ability1;
    Ability ability2;
    Ability ability3;
    Ability ability4;
    Ability ability5;
    Ability ability6;
    Ability ability7;
    Ability ability8;
    Ability ability9;
    Ability ability10;
    Ability ability11;
    public Ability_V2 ability1a;
    public Ability_V2 ability2a;
    public Ability_V2 ability3a;
    public Ability_V2 ability4a;
    public Ability_V2 ability5a;
    public Ability_V2 ability6a;
    public Ability_V2 ability7a;
    public Ability_V2 ability8a;
    public Ability_V2 ability9a;
    public AbilityEff test;
    public Vector3 posTest;
    public PlayerState state;
    //public Vector2 newVect_;
    void Awake(){
        actor = GetComponent<Actor>();
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        posTest = transform.position;
    }
    public override void Start()
    {   
        base.Start();
        if(isLocalPlayer){
            UIManager.playerController = this;

        }
        // ability1 = AbilityData.DoT;
        // ability2 = AbilityData.CastedDamage;
        // ability3 = AbilityData.CastedHeal;
        // ability4 = AbilityData.HoT;
        // ability5 = AbilityData.AoE;
        // ability6 = AbilityData.FreeAbilityIfHit;
        // ability7 = AbilityData.DoubleEffectAbility;
        // ability8 = AbilityData.DelayedDamage;
        // ability9 = AbilityData.Teleport;
        // ability10 = AbilityData.Dash;
        // ability11 = AbilityData.DmgBuffBolt;
        
        

        // Debug.Log(" 1 = DoT");
        // Debug.Log(" 2 = One off Dmg");
        // Debug.Log(" 3 = One off Heal");
        // Debug.Log(" 4 = HoT");
        // Debug.Log(" 5 = AoE (Doesn't disapear yet)");
        // Debug.Log(" 6 = Skill Shot that fires off a 2nd 1 off if it hits an Actor");
        // Debug.Log(" 7 = Ability with 2 effects (1 off heal then DoT)");
        // Debug.Log(" 8 = 1 off Dmg that hits after a delay (4s)");
        // Debug.Log(" R = Teleport");
        // Debug.Log(" F = Dash");
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
                    newVect.x *= (HORIZ_MOVE_ACCEL * Time.deltaTime);
                    newVect.y *= (VERT_MOVE_ACCEL * Time.deltaTime);
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
                        if (ability1a != null)
                actor.castAbility3(ability1a);
            }
                    if (Input.GetKeyDown("2"))
                    {
                        if (ability2a != null)
                actor.castAbility3(ability2a);
            }
                    if (Input.GetKeyDown("3"))
                    {
                        if (ability3a != null)
                    actor.castAbility3(ability3a);
            }
                    if (Input.GetKeyDown("4"))
                    {
                        if (ability4a != null)
                actor.castAbility3(ability4a);
            }
                    if (Input.GetKeyDown("5"))
                    {
                        if (ability5a != null)
                actor.castAbility3(ability5a);
            }
                    if (Input.GetKeyDown("q"))
                    {
                        if (ability6a != null)
                actor.castAbility3(ability6a);
            }
                    if (Input.GetKeyDown("e"))
                    {
                        if (ability7a != null)
                actor.castAbility3(ability7a);
            }
                    if (Input.GetKeyDown("r"))
                    {
                        if (ability8a != null)
                actor.castAbility3(ability8a);
            }
                    if (Input.GetKeyDown("f"))
                    {
                        if (ability9a != null)
                actor.castAbility3(ability9a);
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
        
    }
    [Command]
    void reqAbilityEff(AbilityEff eff){
        Debug.Log("A client reqed an AbilityEff " + eff.effectName);
    }
    void sayAbilityEffectName(AbilityEffect _abilityEffect){
        Debug.Log("This effect " + _abilityEffect.getEffectName() + "  finished!");
    }
    public Vector3 getWorldPointTarget(){
        Vector3 scrnPos = Input.mousePosition;
        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(scrnPos);
        worldPoint.z = 0.0f;
        return worldPoint;
    }
}
