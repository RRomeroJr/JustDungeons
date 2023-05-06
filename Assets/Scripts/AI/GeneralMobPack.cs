using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GeneralMobPack : Multiboss
{
    /*
        This will be put on an actor. It will search for other actors with
        this component and store thier refereces in a list. 
    */
    public float radius = 6.5f;
    public int tempIdentifier;
    public bool eventTested = false;
    //public static UnityEvent<uint> comboEvent = new UnityEvent<uint>();
    
    void Start(){
        partners = new List<Actor>();
        GameManager.instance.OnActorEnterCombat.AddListener(ChainAggro);
        StartCoroutine(SearchForPartners());
    }
    public override IEnumerator SearchForPartners(){
        RaycastHit2D[] castHits = new RaycastHit2D[0];
        searchingForPartners = true;
        while(partners.Count > 0 == false){
            castHits = Physics2D.CircleCastAll(transform.position, radius, Vector2.zero, 0.0f, LayerMask.GetMask("Enemy"));
            foreach(RaycastHit2D hit in castHits){
                if(hit.collider.gameObject != gameObject){
                    if(hit.collider.GetComponent<GeneralMobPack>() != null){
                        partners.Add(hit.collider.GetComponent<Actor>()); 
                    }
                }
            }
            yield return new WaitForSeconds(0.2f);
        }
        searchingForPartners = false;
        OnFindPartners();
    }
    void Update(){
        // if((!searchingForPartners) && (!eventTested)){
        //     if(tempIdentifier == 1){
        //         multibossCoordinator.comboEvent.Invoke(1);
        //         eventTested = true;
        //     }
        // }
    }
    
    void comboAttack1(int _eventInput){
        
        switch(tempIdentifier){
            case(1):
                Debug.Log("Boss mob 1 ONE 1 did an attck!");
                break;
            case(2):
                Debug.Log("And.. Boss mob 2 TWO 2 did their attack!");
                break;
            default:
                Debug.Log("Unknocn mob identifer");
                break;
        }
    }

}