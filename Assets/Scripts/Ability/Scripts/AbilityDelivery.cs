using System.Collections.Generic;
using System.Linq;
using Mirror;
using UnityEngine;

public enum AbilityType
{
    Normal,
    Skillshot = 1,
    Aoe = 2,
    RingAoe = 4,
    LineAoe = 5
}

public class AbilityDelivery : NetworkBehaviour
{
    /*
        0 = Regular tracking delivery
        1 = Skill shot
        2 = AoE w/ time duration WP
        3 = AoE w/ duration target
    */

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
    public int type; // 0 detroys when reaches target, 1 = skill shot
    public float speed;
    public float duration;
    public float tickRate = 1.5f; // an AoE type will hit you every tickRate secs
    public int aoeCap;
    public bool followTarget;
    public bool followCaster;
    public bool trackTarget;
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

    [field: SerializeField] public float RotationsPerSecond { get; private set; }
    [SerializeField] private List<SerializableTuple<float, float>> rotationSequence;

    void OnValidate()
    {
        if (followTarget && followCaster)
        {
            Debug.LogError(gameObject.name + "| Warning followTarget && followCaster. Chose only one. Will default to caster");
        }
    }

    void Start()
    {
        if (isServer)
        {
            aoeDamageableIgnore = new();
            foreach (EffectInstruction eI in eInstructs)
            {
                eI.effect.parentDelivery = this;
            }
            if (type == 1)
            {
                skillshotvector = worldPointTarget - transform.position;
                //Debug.Log(worldPointTarget - transform.position);
                skillshotvector.Normalize();
                skillshotvector = speed * skillshotvector;
                //Debug.Log(gameObject.name + "| skillshot wpT " + worldPointTarget);
                //Debug.Log(gameObject.name + "| skillshotvector set:" + worldPointTarget + " + " + transform.position);
            }
            if (type == 2)
            { // Normal Aoe 
                //gameObject.transform.position = worldPointTarget;
            }
            if (type == 3)
            { // This was for targeted aoes but is now obsolete
                //gameObject.transform.position = target.transform.position;
            }
            if (type == 4)
            { //Ring Aoe
                //gameObject.transform.position = worldPointTarget;
            }
            if (type == 5)
            { //line aoe
              //Debug.Log("Start type 5: LineAoe");
              // transform.right = worldPointTarget - transform.position;         
            }
            if (connectedToCaster)
            {
                float tempDist = GetComponent<Renderer>().bounds.size.x / 2.0f;
                gameObject.transform.position = Vector2.MoveTowards(Caster.transform.position, worldPointTarget, tempDist);
            }
            if (Caster != null)
            {
                //
            }
            if (rotationSequence != null && rotationSequence.Count > 0)
            {
                rotationSequence.Add(new SerializableTuple<float, float>(rotationSequence[0]));
                RotationsPerSecond = rotationSequence[0].Item2;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isServer || !start)
        {
            return;
        }

        if (!other.TryGetComponent(out Actor hitActor))
        {
            if (other.TryGetComponent(out IDamageable damageable) && !CheckIgnoreTarget(damageable))
            {
                foreach (EffectInstruction eI in eInstructs)
                {
                    damageable.Damage(eI.effect.power + Caster.mainStat * eI.effect.powerScale);
                }
                AddToAoeIgnore(damageable, tickRate);
            }
            return;
        }

        if (checkAtFeet && !CheckHitFeet(hitActor))
        {
            return;
        }
        if (checkIgnoreConditons(hitActor) != false)
        {
            return;
        }

        if (type == 0 && hitActor == Target)
        {
            Debug.Log("Sending to " + hitActor.gameObject.name);
            foreach (EffectInstruction eI in eInstructs)
            {
                eI.sendToActor(hitActor.transform, null, Caster);
            }
            Destroy(gameObject);
        }
        if (type == 1 && hitActor != Caster)
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

        //Debug.Log("Trigger stay server and start");
        if (!other.TryGetComponent(out Actor hitActor))
        {
            if (other.TryGetComponent(out IDamageable damageable) && !CheckIgnoreTarget(damageable))
            {
                foreach (EffectInstruction eI in eInstructs)
                {
                    damageable.Damage(eI.effect.power + Caster.mainStat * eI.effect.powerScale);
                }
                AddToAoeIgnore(damageable, tickRate);
            }
            return;
        }
        if (checkAtFeet && !CheckHitFeet(hitActor))
        {
            return;
        }
        if (checkIgnoreConditons(hitActor) != false)
        {
            return;
        }

        //Debug.Log("Actor found and passes conditions");
        if ((type == 2) || (type == 3) || (type == 5))
        {
            if ((hitActor != Caster) || canHitSelf)
            {
                //Debug.Log("Not caster or canHitSelf");
                //Debug.Log(hitActor.getActorName());

                if (checkIgnoreTarget(hitActor) == false)
                {
                    addToAoeIgnore(hitActor, tickRate);

                    Hit(hitActor);
                }

                else
                {
                    //Debug.Log("2||3 no actor found");
                }
                // make version that has a set number for ticks?
            }
        }
        if (type == 4)
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

                    if (checkIgnoreTarget(hitActor) == false)
                    {
                        addToAoeIgnore(hitActor, tickRate);

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
    void Hit(Actor _target)
    {
        foreach (EffectInstruction eI in eInstructs){
            eI.sendToActor(_target.transform, null, Caster);
        }
        foreach(BuffScriptableObject b in buffs)
        {
            _target.buffHandler.AddBuff(b);
        }
    }
    void FixedUpdate()
    {
        if (!isServer || !start)
        {
            return;
        }
        if (type == 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, Target.position, speed);
        }
        else if (type == 1)
        {
            transform.position = (Vector2)transform.position + skillshotvector;
        }
        else if (type == 5)
        {
            if (trackTarget)
            {
                Vector3 targetLocation = Target != null ? Target.position : worldPointTarget;
                if (targetLocation != null)
                {
                    transform.right = Vector3.Normalize(targetLocation - transform.position);
                }
            }
        }
        // Rotation logic
        if (!Mathf.Approximately(RotationsPerSecond, 0))
        {
            Vector3 rotation = new Vector3(0, 0, RotationsPerSecond * 360) * Time.fixedDeltaTime;
            transform.Rotate(rotation, Space.World);
        }
    }

    void Update()
    {
        if (isServer)
        {
            if (followCaster && Caster != null)
            {
                transform.position = Caster.transform.position;
            }
            else if (followTarget && Target != null)
            {
                transform.position = Target.position;
            }

            if ((useDisconnectTimer) && (disconnectTimer <= 0))
            {
                followTarget = false;
                followCaster = false;
            }
            if (start)
            {
                updateTargetCooldowns();
                if (!ignoreDuration)
                {
                    duration -= Time.deltaTime;
                    if (duration <= 0)
                    {
                        //Debug.Log("Destroying AoE");        
                        Destroy(gameObject);
                    }
                }

                if (type == 4)
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
            if (rotationSequence != null && rotationSequence.Count > 0)
            {
                rotationSequence[0].Item1 -= Time.deltaTime;
                // Prep next element of sequence by adding copy to end of list,
                // adding remaining time to next element, remove current element and update RotationsPerSecond
                if (rotationSequence[0].Item1 <= 0)
                {
                    rotationSequence.Add(new SerializableTuple<float, float>(rotationSequence[1]));
                    rotationSequence[1].Item1 += rotationSequence[0].Item1;
                    rotationSequence.RemoveAt(0);
                    RotationsPerSecond = rotationSequence[0].Item2;
                }
            }
        }
        if (useDisconnectTimer)
        {
            disconnectTimer -= Time.deltaTime;
        }
    }

    Vector3 getWorldPointTarget()
    {
        Vector3 scrnPos = Input.mousePosition;
        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(scrnPos);
        worldPoint.z = 0.0f;
        return worldPoint;
    }

    public bool checkIgnoreTarget(Actor _target)
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

    bool checkIgnoreConditons(Actor _hitActor)
    {
        //returns true if the _histActor should be ignored
        if (_hitActor == null)
        {
            Debug.Log("_hitActor null");
            return false;
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
        // return GetComponent<Collider2D>().OverlapPoint(_hitActor.GetComponent<Collider2D>().bounds.BottomCenter());
    }

    void addToAoeIgnore(Actor _target, float _remainingtime)
    {
        aoeActorIgnore.Add(new TargetCooldown(_target, _remainingtime));
    }

    void AddToAoeIgnore(IDamageable target, float remainingtime)
    {
        aoeDamageableIgnore.Add(new DamageableCooldown(target, remainingtime));
    }

    void updateTargetCooldowns()
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
    public void setSafeZoneScale(Vector2 _hostScale)
    {
        transform.GetChild(0).transform.localScale = _hostScale;
    }

    [ClientRpc]
    public void setSafeZonePosistion(Vector2 _hostPos)
    {
        transform.GetChild(0).transform.position = _hostPos;
    }
}
