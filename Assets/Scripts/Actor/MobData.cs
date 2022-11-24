using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName="MobData", menuName = "HBCsystem/MobData")]
public class MobData : ScriptableObject
{
    /*   
        Container for all abilities used by players? Maybe this will be broken up into classes later
    */
    public static MobData _inst;
    
    public static MobData instance{ get { 
        if(!_inst){
            MobData[] findResult = (Resources.FindObjectsOfTypeAll(typeof (MobData)) as MobData[]);
            if(findResult.Length > 0){
                Debug.Log("Assigning 1st MobData instance to _inst");
                _inst = findResult[0];
            }
            if(findResult.Length > 1)
                Debug.LogWarning("Mutiple MobData objects found");
        }
        if(!_inst){
            Debug.LogError("MobData: No MobData instance found.");
            _inst = CreateInstance<MobData>();
        }
        return _inst;
    }}
    /*
    public static Ability CastedDamage = new Ability("Testerbolt", AbilityEffectData.oneOffDamageEffect, 0, _castTime: 1.5f);
    public static Ability DoT = new Ability("Debugger\'s Futility", AbilityEffectData.dotEffect, 0, 0.0f, 3.5f);
    public static Ability CastedHeal = new Ability("Quality Assured", AbilityEffectData.oneOffHealEffect, 0, 1.5f, 4.2f);
    public static Ability HoT = new Ability("Sisyphean Resolve", AbilityEffectData.hotEffect, -1, 0.0f);

    /*public static Ability DoubleEffectAbility = new Ability("Double Effect-Bolt",
                                                        new List<AbilityEffectPreset>{AbilityEffectData.oneOffDamageEffect, AbilityEffectData.dotEffect},
                                                            0, 1.5f);
                                                        
    public static Ability FreeAbilityIfHit = new Ability("Testerbolt x2", AbilityEffectData.DmgWithFollowUpEffect, 1, 2.5f);
    public static Ability AoE = new Ability("TB as AoE", AbilityEffectData.oneOffDamageEffect, 2, 0.5f, _duration: 5.0f);
    public static Ability DelayedDamage = new Ability("Hits aft 4.0s", AbilityEffectData.DelayedOneOffEffect, 0, _castTime: 1.5f);

    public static Ability Teleport = new Ability("Teleport", AbilityEffectData.TeleportEffect, -2);
    public static Ability Dash = new Ability("Dash", AbilityEffectData.DashEffect, -2);

    //public static Ability AoE2 = new Ability("TB as AoE2", AbilityEffectData.oneOffDamageEffect, 3);
    public static Ability DmgActorBolt = new Ability("Dmg Actor bolt", AbilityEffectData.ActorNextTB, -1, _castTime: 1.5f);
    public static Ability DoubleEffectAbility = new Ability("Double Effect-Bolt",
                                                        new List<AbilityEffectPreset>(){AbilityEffectData.oneOffDamageEffect, AbilityEffectData.dotEffect},
                                                            0, 1.5f);
   */ public List<Actor> MobPrefabList;
    public void OnValidate(){
        _inst = this;
        Debug.Log("MobData validate, but doing nothing"); // This won't show up out of play mode for some reason
        //setIDs();
    }
    public void setIDs(){
            if(MobPrefabList.Count > 0){
                for (int i = 0; i < MobPrefabList.Count; i++)
                {
                    MobPrefabList[i].mobId = i;
                }
            }
        }
    public Actor find(int _mobId){
            foreach(Actor Actor in MobPrefabList){
                if(Actor.mobId == _mobId){
                    return Actor;
                }
            }
            return null;
    }
}
