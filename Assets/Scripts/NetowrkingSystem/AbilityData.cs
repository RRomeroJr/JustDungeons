using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName="AbilityData", menuName = "HBCsystem/AbilityData")]
public class AbilityData : ScriptableObject
{
    /*   
        Container for all abilities used by players? Maybe this will be broken up into classes later
    */
    public static AbilityData _inst;
    public Ability_V2 AutoAttack;
    public static AbilityData instance{ get { 
        if(!_inst){
            AbilityData[] findResult = (Resources.FindObjectsOfTypeAll(typeof (AbilityData)) as AbilityData[]);
            if(findResult.Length > 0){
                Debug.Log("Assigning 1st instance to _inst");
                _inst = findResult[0];
            }
            if(findResult.Length > 1)
                Debug.LogWarning("Mutiple AbilityData objects found");
        }
        if(!_inst){
            Debug.LogError("ad: No ad instance found.");
            _inst = CreateInstance<AbilityData>();
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
    public static Ability DmgBuffBolt = new Ability("Dmg buff bolt", AbilityEffectData.BuffNextTB, -1, _castTime: 1.5f);
    public static Ability DoubleEffectAbility = new Ability("Double Effect-Bolt",
                                                        new List<AbilityEffectPreset>(){AbilityEffectData.oneOffDamageEffect, AbilityEffectData.dotEffect},
                                                            0, 1.5f);
   */ public List<Ability_V2> abilityList;
    public void OnValidate(){
        _inst = this;
        Debug.Log("ad validate"); // This won't show up out of play mode for some reason
        setIDs();
    }
    public void setIDs(){
            if(abilityList.Count > 0){
                for (int i = 0; i < abilityList.Count; i++)
                {
                    abilityList[i].id = i;
                }
            }
        }
    public Ability_V2 find(int _id){
            foreach(Ability_V2 ability in abilityList){
                if(ability.id == _id){
                    return ability;
                }
            }
            return null;
    }
    public Ability_V2 findByName(string _name){
            foreach(Ability_V2 ability in abilityList){
                if(ability.name == _name){
                    return ability;
                }
            }
            return null;
    }
}
