using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class Arena : NetworkBehaviour
{
    public bool killPlayerOnExit = false;
    public List<Actor> mobList;
    public bool destroyIfListEmpty = true;
    [SerializeField]
    protected GameObject safezone; 
    public GameObject Safezone{
        get{return safezone;}
    }
    public bool killPlayerInHurtzone;
    public bool usePercentHealthDmg;
    public float percentHealthDmg;
    public List<TargetCooldown> actorIgnore;
    public float hitCooldownTime = 3.0f;

    
    public void OnTriggerExit2D(Collider2D other)
    {   if(killPlayerOnExit){
            other.gameObject.GetComponent<Actor>().Health = 0;
        }
        
    }
    public virtual void OnTriggerStay2D(Collider2D other)
    {   
        Actor otherActor = other.GetComponent<Actor>();
        if(otherActor == null){
            return;
        }
        if(otherActor.tag != "Player"){
            return;
        }
        if(otherActor.State == ActorState.Dead){
            return;
        }
        if(IsInSafezone(otherActor)){
            return;
        }
        if(checkIgnoreTarget(otherActor)){
            return;
        }

        if(killPlayerInHurtzone){
            otherActor.setHealth(0);
            addToIgnore(otherActor, hitCooldownTime);
        }
        else if(usePercentHealthDmg){
            otherActor.damageValue((int)(otherActor.getMaxHealth() * percentHealthDmg));
            addToIgnore(otherActor, hitCooldownTime);
        }
    
    }
    void Update(){
        if(isServer){
            updateTargetCooldowns();
            if(destroyIfListEmpty){
                if(mobList.Count <= 0){
                    Destroy(gameObject);
                }
            }
        }
        
        
    }
    void FixedUpdate(){
        for (int i = 0; i < mobList.Count; i++)
        {
            if(mobList[i] == null){
                mobList.RemoveAt(i);
            }
        }
    }   
    public virtual bool IsInSafezone(Actor _actor){
        Debug.Log("Base IsInSafezone called");
        return true;
    }
    void OnValidate(){

    }
    public void Start(){

    }
    void addToIgnore(Actor _target, float _remainingtime){
        actorIgnore.Add(new TargetCooldown(_target, _remainingtime));
    }
    void updateTargetCooldowns(){
        if(actorIgnore.Count > 0){
            for(int i = 0; i < actorIgnore.Count; i++){
                if(actorIgnore[i].remainingTime > 0)
                    actorIgnore[i].remainingTime -= Time.deltaTime;
                else
                    actorIgnore.RemoveAt(i);
            }
        }
    }
    public bool checkIgnoreTarget(Actor _target){
        if(actorIgnore.Count > 0){
            for(int i = 0; i < actorIgnore.Count; i++){
                if(actorIgnore[i].actor == _target){
                    //Debug.Log(actorIgnore[i].actor.actorName +"At [" + i.ToString() + "] is on cooldown!");
                    return true;
                }
            }
            return false;
        }
        else{
            return false;
        }
    }
}
