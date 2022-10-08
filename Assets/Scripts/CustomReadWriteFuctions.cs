using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public static class CustomReadWriteFuctions
{
    public static void WriteAbilityEff(this NetworkWriter writer, AbilityEff effect)
    {
       
       writer.WriteInt(effect.id);
    }

    public static AbilityEff ReadAbilityEff(this NetworkReader reader)
    {
   
        return AbilityEffectData.instance.find(reader.ReadInt());
    }
    public static void WriteAbility(this NetworkWriter writer, Ability_V2 ability){
       //Debug.Log("Writting ability: " + (ability != null ? (ability.getName() + " id: " + ability.id.ToString()) : "NULL"));
       writer.WriteInt(ability.id);
       //Debug.Log("Leaving WriteAbility");
    }
    public static Ability_V2 ReadAbility(this NetworkReader reader){
        //Debug.Log("ReadAbility");
        //Debug.Log(AbilityData.instance == null ? "No ad" : "Read Ability: ad found");
        //Debug.Log(reader == null ? "reader null" : "reader OK");
        int abilityID = reader.ReadInt();
        Ability_V2 result = null;
        if(AbilityData.instance != null){
            if(AbilityData.instance.abilityList != null){
                if(abilityID < AbilityData.instance.abilityList.Count){
                    result = AbilityData.instance.find(abilityID);
                    
                }
                else{
                    Debug.LogError("abilityID > than the length of abilityaList");
                }
            }else{
                Debug.LogError("No abilityList in ad");
            }
        }else{
            Debug.LogError("ad instance == NULL");
        }

        //Debug.Log("ad result: " + (result != null ? (result.getName() + " id: " + result.id.ToString()) : "NULL"));
        return result;
    }
    
    public static void WriteActor(this NetworkWriter writer, Actor actor)
    {
        // if(actor ==null){
        //     Debug.Log("Serializing null Actor");
        //     //Debug.Log( "actor " + actor.getActorName() + "does not have a network identity");
        // }

        NetworkIdentity networkIdentity = actor != null
            ? actor.gameObject.GetComponent<NetworkIdentity>()
            : null;
        writer.WriteNetworkIdentity(networkIdentity);
    }

    public static Actor ReadActor(this NetworkReader reader)
    {
        
        NetworkIdentity networkIdentity = reader.ReadNetworkIdentity();
        // if(networkIdentity == null){
        //     Debug.Log("network ID null");
        // }
        Actor actor = networkIdentity != null
            ? networkIdentity.GetComponent<Actor>()
            : null;

        return actor;
    }

}
