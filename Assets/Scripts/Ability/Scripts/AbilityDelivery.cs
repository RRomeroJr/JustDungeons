using System.Collections.Generic;
using System.Linq;
using Mirror;
using UnityEngine;

public enum AbilityType
{
    Normal,
    Skillshot = 1,
    Aoe = 2,
    TargetedAoe = 3, // Not sure if this is used
    RingAoe = 4,
    LineAoe = 5,
    LineAoeSimple = 6
}

[System.Serializable]
public struct RotationElement
{
    public float Duration;
    public float RotationsPerSecond;
}

public class AbilityDelivery : NetworkBehaviour
{
    /*
        0 = Regular tracking delivery
        1 = Skill shot
        2 = AoE w/ time duration WP
        3 = AoE w/ duration target
    */

    // public Actor caster; 
    // public Actor Caster { 
    //     get {return caster;}
    //     set {caster = value;}
    // }
    // public Transform target;
    // public Transform Target { 
    //     get {return target;}
    //     set {target = value;}
    // }
    public Actor Caster { get; set; }
    public Transform Target { get; set; }
    public Vector3 worldPointTarget;
    public List<TargetCooldown> aoeActorIgnore;
    private List<DamageableCooldown> aoeDamageableIgnore;

    // [SerializeField]public List<AbilityEffect> abilityEffects;
    [SerializeField] public List<EffectInstruction> eInstructs;
    [SerializeField] public List<BuffScriptableObject> buffs;
    public bool connectedToCaster = false;
    public float delayTimer = 0.0f;
    public bool start = true;
    public AbilityType type; // 0 detroys when reaches target, 1 = skill shot
    public float speed;
    public float duration;
    public float tickRate = 1.5f; // an AoE type will hit you every tickRate secs
    public int aoeCap;
    public bool followTarget;
    public bool followCaster;
    public bool useDisconnectTimer = false;
    public float disconnectTimer;
    public bool ignoreDuration = true;
    public float innerCircleRadius;
    public Vector2 safeZoneCenter;
    public Vector2 skillshotvector;
    public LayerMask hitMask = 192; //Should be player | enemy

    public bool hitHostile = true;
    public bool hitFriendly = true;
    public bool canHitSelf = false;
    public bool checkAtFeet = false;
    public bool onlyHitTarget = false;

    [SerializeField] private List<RotationElement> rotationSequence;

    [Header("Line Aoe Specific")]
    [Tooltip("Whether or not Line Aoes should point towards their target or not")]
    public bool trackTarget;

    private AbilityDeliveryTransformationController _movementController;

    void OnValidate()
    {
        if (followTarget && followCaster)
        {
            Debug.LogError(gameObject.name + "| Warning followTarget && followCaster. Chose only one. Will default to caster");
        }
    }

    void Start()
    {
        if (!isServer) { return; }
        // Server only logic below

        aoeDamageableIgnore = new();
        foreach (EffectInstruction eI in eInstructs)
        {
            eI.effect.parentDelivery = this;
        }
        if (type == AbilityType.Skillshot)
        {
            skillshotvector = worldPointTarget - transform.position;
            //Debug.Log(worldPointTarget - transform.position);
            skillshotvector.Normalize();
            skillshotvector = speed * skillshotvector;
            //Debug.Log(gameObject.name + "| skillshot wpT " + worldPointTarget);
            //Debug.Log(gameObject.name + "| skillshotvector set:" + worldPointTarget + " + " + transform.position);
        }
        if (connectedToCaster)
        {
            float tempDist = GetComponent<Renderer>().bounds.size.x / 2.0f;
            gameObject.transform.position = Vector2.MoveTowards(Caster.transform.position, worldPointTarget, tempDist);
        }
        _movementController = new AbilityDeliveryTransformationController(this, rotationSequence);
    }

    void Update()
    {
        if (isServer)
        {
            if ((useDisconnectTimer) && (disconnectTimer <= 0))
            {
                followTarget = false;
                followCaster = false;
            }
            if (start)
            {
                UpdateTargetCooldowns();
                if (!ignoreDuration)
                {
                    duration -= Time.deltaTime;
                    if (duration <= 0)
                    {
                        //Debug.Log("Destroying AoE");        
                        Destroy(gameObject);
                    }
                }

                if (type == AbilityType.RingAoe)
                {
                    safeZoneCenter = transform.GetChild(0).transform.position;
                }
            }
            else
            {
                delayTimer -= 1.0f * Time.deltaTime;
                if (delayTimer <= 0.0f)
                {
                    start = true;
                }
            }
        }
        if (useDisconnectTimer)
        {
            disconnectTimer -= Time.deltaTime;
        }
    }

    void FixedUpdate()
    {
        if (!isServer)
        {
            return;
        }
        /*
            Richie: I need Aoes to track before they are actually active
            I use it as a rudimentary targeting indicator.

            You add a delay, which prevents start from becoming true for the
            duration. But durring that time the Aoe will track.

            Move is the only one I need. Sorry if this change broke something else

            I left the others below this check in hopes not to break more things
        */
        _movementController.Move();
        if (!start)
        {
            return;
        }

        _movementController.TrackTarget();
        _movementController.Rotate();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isServer || !start)
        {
            return;
        }
        if (type is not (AbilityType.Normal or AbilityType.Skillshot))
        {
            return;
        }

        // IDamageable Logic
        if (HitDamageable(other))
        {
            return;
        }

        // Actor Logic
        if (other.TryGetComponent(out Actor hitActor) == false)
        {
            return;
        }
        if (checkAtFeet && !CheckHitFeet(hitActor))
        {
            return;
        }
        if (CheckIgnoreConditons(hitActor) != false)
        {
            return;
        }

        if (CheckIgnoreTarget(hitActor) == false)
        {
            foreach (EffectInstruction eI in eInstructs)
            {
                eI.sendToActor(hitActor.transform, null, Caster);
            }
            Destroy(gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!isServer || !start)
        {
            return;
        }

        // IDamageable Logic
        if (HitDamageable(other))
        {
            return;
        }

        // Actor Logic
        if (other.TryGetComponent(out Actor hitActor) == false)
        {
            return;
        }
        if (checkAtFeet && !CheckHitFeet(hitActor))
        {
            return;
        }
        if (CheckIgnoreConditons(hitActor) != false)
        {
            return;
        }

        //Debug.Log("Actor found and passes conditions");
        if (type is AbilityType.Aoe or AbilityType.TargetedAoe or AbilityType.LineAoe or AbilityType.LineAoeSimple)
        {
            if ((hitActor != Caster) || canHitSelf)
            {
                //Debug.Log("Not caster or canHitSelf");
                //Debug.Log(hitActor.getActorName());

                if (CheckIgnoreTarget(hitActor) == false)
                {
                    AddToAoeIgnore(hitActor, tickRate);

                    Hit(hitActor);
                }

                else
                {
                    //Debug.Log("2||3 no actor found");
                }
                // make version that has a set number for ticks?
            }
        }
        if (type is AbilityType.RingAoe)
        {
            //Debug.Log("type 4 onTiggerStay");
            if ((hitActor != Caster) || canHitSelf)
            {
                //Debug.Log("Actor found and not caster");
                Vector2 hitCheckPoint;
                if (checkAtFeet)
                {
                    hitCheckPoint = hitActor.GetComponent<Collider2D>().bounds.BottomCenter();
                }
                else
                {
                    hitCheckPoint = other.GetComponent<Collider2D>().bounds.center;
                }
                float dist = Vector2.Distance(hitCheckPoint, safeZoneCenter);
                if (dist > innerCircleRadius)
                {
                    // Debug.DrawLine(other.GetComponent<Collider2D>().bounds.center, safeZoneCenter, Color.red);

                    if (CheckIgnoreTarget(hitActor) == false)
                    {
                        AddToAoeIgnore(hitActor, tickRate);

                        if (eInstructs.Count > 0)
                        {
                            foreach (EffectInstruction eI in eInstructs)
                            {
                                eI.sendToActor(hitActor.transform, null, Caster);
                            }
                        }
                    }
                }
                else
                {
                    // Debug.DrawLine(other.GetComponent<Collider2D>().bounds.center, safeZoneCenter, Color.green);
                }
                // make version that has a set number for ticks?
            }
        }
    }

    private bool HitDamageable(Collider2D collider)
    {
        if (collider.TryGetComponent(out IDamageable damageable) == false)
        {
            return false;
        }
        if (CheckIgnoreTarget(damageable) == false)
        {
            foreach (EffectInstruction eI in eInstructs)
            {
                damageable.Damage(eI.effect.power + Caster.mainStat * eI.effect.powerScale);
            }
            AddToAoeIgnore(damageable, tickRate);
        }
        return true;
    }

    void Hit(Actor _target)
    {
        foreach (EffectInstruction eI in eInstructs)
        {
            eI.sendToActor(_target.transform, null, Caster);
        }
        foreach (BuffScriptableObject b in buffs)
        {
            _target.buffHandler.AddBuff(b);
        }
    }

    Vector3 getWorldPointTarget()
    {
        Vector3 scrnPos = Input.mousePosition;
        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(scrnPos);
        worldPoint.z = 0.0f;
        return worldPoint;
    }

    public bool CheckIgnoreTarget(Actor _target)
    {
        foreach (var targetCooldown in aoeActorIgnore)
        {
            if (targetCooldown.actor == _target)
            {
                //Debug.Log(aoeActorIgnore[i].actor.actorName +"At [" + i.ToString() + "] is on cooldown!");
                return true;
            }
        }
        return false;
    }

    public bool CheckIgnoreTarget(IDamageable target)
    {
        return aoeDamageableIgnore.Any(x => x.damageable == target);
    }

    bool CheckIgnoreConditons(Actor _hitActor)
    {
        if (onlyHitTarget && (_hitActor.transform != Target))
        {
            return true;
        }
        if ((canHitSelf == false) && (_hitActor == Caster))
        {
            return true;
        }

        if (hitMask == (hitMask | (1 << _hitActor.gameObject.layer)) == false)
        { //if hit does not match mask
            //Debug.Log(_hitActor.getActorName() + " did not match hitMask. go: " + gameObject.name);
            return true;
        }
        if (Caster != null)
        {
            if (hitHostile && HBCTools.areHostle(Caster.transform, _hitActor.transform) == true)
            {
                //Debug.Log(caster.getActorName() + " & " + _hitActor.getActorName() + " are not hostile");
                return false;
            }
            if (hitFriendly && HBCTools.areHostle(Caster.transform, _hitActor.transform) == false)
            {
                //Debug.Log(caster.getActorName() + " & " + _hitActor.getActorName() + " are not friendly");
                return false;
            }
        }
        else
        {
            return false;
        }

        if ((!hitHostile) && (!hitFriendly))
        {
            Debug.Log("Warning delivery " + gameObject.name + " doesn't hit hostiles or friendlies");
        }

        return true;
    }

    bool CheckHitFeet(Actor _hitActor)
    {
        //returns true if the _histActor should be ignored
        if (GetComponent<Collider2D>().OverlapPoint(_hitActor.GetComponent<Collider2D>().bounds.BottomCenter()))
        {
            Debug.DrawLine(_hitActor.GetComponent<Collider2D>().bounds.BottomCenter(), transform.position, Color.red);
            return true;
        }
        else
        {
            Debug.DrawLine(_hitActor.GetComponent<Collider2D>().bounds.BottomCenter(), transform.position, Color.green);
            return false;
        }
    }

    void AddToAoeIgnore(Actor _target, float _remainingtime)
    {
        aoeActorIgnore.Add(new TargetCooldown(_target, _remainingtime));
    }

    void AddToAoeIgnore(IDamageable target, float remainingtime)
    {
        aoeDamageableIgnore.Add(new DamageableCooldown(target, remainingtime));
    }

    void UpdateTargetCooldowns()
    {
        for (int i = aoeActorIgnore.Count - 1; i >= 0; i--)
        {
            if (aoeActorIgnore[i].remainingTime > 0)
                aoeActorIgnore[i].remainingTime -= Time.deltaTime;
            else
                aoeActorIgnore.RemoveAt(i);
        }
        for (int i = aoeDamageableIgnore.Count - 1; i >= 0; i--)
        {
            if (aoeDamageableIgnore[i].remainingTime > 0)
                aoeDamageableIgnore[i].remainingTime -= Time.deltaTime;
            else
                aoeDamageableIgnore.RemoveAt(i);
        }
    }

    [ClientRpc]
    public void SetSafeZoneScale(Vector2 _hostScale)
    {
        transform.GetChild(0).transform.localScale = _hostScale;
    }

    [ClientRpc]
    public void SetSafeZonePosistion(Vector2 _hostPos)
    {
        transform.GetChild(0).transform.position = _hostPos;
    }
}
