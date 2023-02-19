using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using OldBuff;

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
        // if(actor != null){
        //     Debug.Log(actor.getActorName());
        // }
        return actor;
    }
    public static void WriteBuff(this NetworkWriter writer, Buff buff)
    {
        writer.WriteInt(buff.id); // get what buff it is

        //get the rest of the important information
        writer.WriteFloat(buff.getDuration());
        writer.WriteFloat(buff.getTickRate());
        //particles
        writer.WriteBool(buff.isStackable());
        writer.WriteBool(buff.isRefreshable());
        writer.WriteFloat(buff.lastTick);
        writer.WriteFloat(buff.getRemainingTime());
        writer.WriteBool(buff.getStart());
        writer.WriteBool(buff.firstFrame);
        writer.WriteActor(buff.getCaster());
        writer.WriteActor(buff.getActor());
        writer.WriteActor(buff.target);
        writer.WriteUInt(buff.getStacks());

    }
    public static Buff ReadBuff(this NetworkReader reader){
        //Debug.Log("ReadAbility");
        //Debug.Log(AbilityData.instance == null ? "No ad" : "Read Ability: ad found");
        //Debug.Log(reader == null ? "reader null" : "reader OK");
        int buffID = reader.ReadInt();
        Buff result = null;
        if(BuffData.instance != null){
            if(BuffData.instance.buffList != null){
                if(buffID < BuffData.instance.buffList.Count){
                    result = BuffData.instance.find(buffID);
                    
                }
                else{
                    Debug.LogError("buffID > than the length of buffList");
                }
            }else{
                Debug.LogError("No buffList in BuffData");
            }
        }else{
            Debug.LogError("BuffData instance == NULL");
        }
        Buff buffClone;
        buffClone = result.clone();
        //Debug.Log("ad result: " + (result != null ? (result.getName() + " id: " + result.id.ToString()) : "NULL"));

        buffClone.setDuration(reader.ReadFloat());
        buffClone.setTickRate(reader.ReadFloat());
        buffClone.stackable = reader.ReadBool();
        buffClone.refreshable = reader.ReadBool();
        buffClone.lastTick = reader.ReadFloat();
        buffClone.setRemainingTime(reader.ReadFloat());
        buffClone.setStart(reader.ReadBool());
        buffClone.firstFrame = reader.ReadBool();
        buffClone.setCaster(reader.ReadActor());
        Actor buffActor = reader.ReadActor();
        Debug.Log(buffActor.getActorName() + " and type is : " + buffActor.GetType().ToString());
        buffClone.setActor(buffActor);
        buffClone.target = reader.ReadActor();
        buffClone.setStacks(reader.ReadUInt());

        return buffClone;
    }

    public static void WriteBuffSO(this NetworkWriter writer, BuffScriptableObject buff)
    {
        writer.WriteString(buff.name);
    }

    public static BuffScriptableObject ReadBuffSO(this NetworkReader reader)
    {
        return Resources.Load<BuffScriptableObject>(reader.ReadString());
    }

}
