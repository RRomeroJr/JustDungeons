// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using Mirror;

// public class Warrior : ClassBase
// {
//     [SyncVar]
//     public int rageMax = 100;
//     [SyncVar]
//     public int rage = 100;
    
//     public ClassResourceType rageType;
//     [SyncVar]
//     public ClassResourceTest rageResourceTest;
//     public ClassResourceTest rageResourceTesttHolder2;
//     public ClassResourceTest rageResourceTesttHolder;
    
//     // public SyncList<ClassResource> resources = new SyncList<ClassResource>();
//     protected override void Start(){

//         rageResourceTesttHolder = rageResourceTest;
//     }
//     protected override void Update(){
//         if(Input.GetKeyDown("p")){
//             if(isLocalPlayer){
//                 increaseRage();
//             }
            
//         }
//     }
//     [Command]
//     void increaseRage(){
//         if(rageResourceTest == rageResourceTesttHolder){
//             rageResourceTest = rageResourceTesttHolder2;
//         }
//         else{
//             rageResourceTest = rageResourceTesttHolder;
//         }
//         //rageResourceTest.amount += 10;
//         rageTestIncread();
//     }
//     [ClientRpc]
//     void rageTestIncread(){
//         Debug.Log("rageResourceTest icreaseRage completed");
//     }
//     public override bool damageResource(AbilityResource _cost){
//          if(_cost != null){
//             if(_cost.arType == AbilityResourceTypes.Health){
//                 actor.setHealth(actor.getHealth() - _cost.amount);
//                 return true;
//             }
//             else{
//                 if(_cost.arType == AbilityResourceTypes.Rage){
//                         rage -= _cost.amount;
//                         if(rage < 0){
//                             rage = 0;
//                         }
//                         return true;
//                 }
//                 // foreach(ActorResource ar in actor.resources){
//                 //     if(ar.arType == _cost.arType){
//                 //         ar.amount -= _cost.amount;
//                 //         if(ar.amount < 0){
//                 //             ar.amount = 0;
//                 //         }
//                 //         return true;
//                 //     }
//                 // }
//             }
//         }
//         return false;
//     }
//     public override bool restoreResource(ClassResourceType _crt, int _amount){
//          if(_crt != null){
//             // foreach(ClassResource cr in resources){

//             //     //Cheack for health type if so restore health
//             //     //else
//             //     if(_crt == cr.crType){
                    
//             //         cr.amount += _amount;
//             //         // Debug.Log("cr.amount += _amount;| " +cr.amount.ToString());
//             //         if(cr.amount >  cr.max){
//             //             cr.amount = cr.max;
//             //         }
//             //         // Debug.Log("before return true; " +cr.amount.ToString());
//             //         return true;               
//             //     }
//             // }
            

//             //Cheack for health type if so restore health
//             //else
//             if(_crt == rageResource.crType){
                
//                 rageResource.amount += _amount;
//                 // Debug.Log("cr.amount += _amount;| " +cr.amount.ToString());
//                 if(rageResource.amount >  rageResource.max){
//                     rageResource.amount = rageResource.max;
//                 }
//                 // Debug.Log("before return true; " +cr.amount.ToString());
//                 return true;               
//             }
            
//          }
//         return false;
//     }
    
//     public override bool hasResources(List<AbilityResource> _arList){
//         foreach(AbilityResource ar in _arList){
//             if((ar.arType != AbilityResourceTypes.Health)&&(ar.arType != AbilityResourceTypes.Rage)){
//                 return false;
//             }
//         }
//         return true;
//     }
// }
