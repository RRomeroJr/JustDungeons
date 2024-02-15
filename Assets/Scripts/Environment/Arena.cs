using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class Arena : NetworkBehaviour
{
    public bool killPlayerOnExit = false;
    public List<Actor> mobList;
    public List<Actor> playerList;
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

    
    public void OnTriggerEnter2D(Collider2D other)
    {   
        if(!isServer)
        {
            return;
        }
        Actor otherActor = other.GetComponent<Actor>();
        if(otherActor == null)
        {
            return;
        }
        if(otherActor.tag != "Player")
        {
            return;
        }
        if(playerList.Contains(otherActor))
        {
            return;
        }
        playerList.Add(otherActor);
        
    }
    public void OnTriggerExit2D(Collider2D other)
    {   
        if(!isServer)
        {
            return;
        }
        Actor otherActor = other.GetComponent<Actor>();
        if(otherActor == null){
            return;
        }
        if(otherActor.tag != "Player"){
            return;
        }
        if(IsInSafezone(otherActor)){
            return;
        }
        if(killPlayerOnExit){
            otherActor.Health = 0;
            if(!PlayersAlive())
            {
                Destroy(gameObject);
            }
        }
        if(playerList.Contains(otherActor))
        {
            playerList.Remove(otherActor);
        }
        
    }
    public virtual void OnTriggerStay2D(Collider2D other)
    {   
        if(!isServer)
        {
            return;
        }
        Actor otherActor = other.GetComponent<Actor>();
        if(otherActor == null){
            return;
        }
        if(otherActor.tag != "Player"){
            return;
        }
        if(otherActor.state == ActorState.Dead){
            return;
        }
        if(IsInSafezone(otherActor)){
            return;
        }
        if(checkIgnoreTarget(otherActor)){
            return;
        }

        if(killPlayerInHurtzone){
            otherActor.Health = 0;
            addToIgnore(otherActor, hitCooldownTime);
        }
        else if(usePercentHealthDmg){
            otherActor.damageValue((int)(otherActor.MaxHealth * percentHealthDmg));
            addToIgnore(otherActor, hitCooldownTime);
        }
        
        if(playerList.Count > 0 && !PlayersAlive())
        {
            Destroy(gameObject);
        }

    }
    /// <summary>
    ///	Returns true if atleast 1 player alive
    /// </summary>
    bool PlayersAlive()
    {
        foreach(Actor a in playerList)
        {
            if(a.state == ActorState.Alive)
            {
                return true;
            }
        }
        return false;
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
        if(!isServer)
        {
            return;
        }
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
