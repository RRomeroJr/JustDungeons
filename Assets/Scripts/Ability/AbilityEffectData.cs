using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
[CreateAssetMenu(fileName="AbilityEffectData", menuName = "HBCsystem/AbilityEffectData")]
public class AbilityEffectData : ScriptableObject
{
    /*  
        Container of Ability Effects and start/ hit/ finish effects.
        Remeber to put particles in the resource folder or they won't be found!
    */
        public static AbilityEffectData instance;
        //                           (AbilityEffect effectName, Ability Type, Power, Duration, Tick Rate, GameObject particles) || 0=dmg, 1=heal
        /*public static AbilityEffectPreset oneOffDamageEffect = new AbilityEffectPreset("1 Off Dmg Effect", 0, 8.0f, 0.0f, 0.0f, _hitAction: standardDamage, _id: 0);
        public static AbilityEffectPreset dotEffect = new AbilityEffectPreset("DoT Effect", 2, 30.0f, 9.0f, 3.0f,  _id: 1);// damage ^^
        public static AbilityEffectPreset oneOffHealEffect = new AbilityEffectPreset("1 Off Heal Effect", 1, 13.0f, 0.0f, 0.0f, _id: 2);
        public static AbilityEffectPreset hotEffect = new AbilityEffectPreset("HoT Effect", 3, 25.0f, 4.0f, 1.0f, _id: 3);// heals ^^
        public static AbilityEffectPreset DmgWithFollowUpEffect = new AbilityEffectPreset("1 off x2 Effect", 0, 8.0f, 0.0f, _finishAction: secondaryTestboltFinish, _id: 4);
        public static AbilityEffectPreset DelayedOneOffEffect = new AbilityEffectPreset("Delayed 1 off Effect", 0, 0.0f, 10.0f, _startAction:buffNextTB_add, _finishAction: buffNextTB_remove, _id: 5);
        public static AbilityEffectPreset TeleportEffect = new AbilityEffectPreset("Teleport effect", 4, 0.0f, _id: 6);
        public static AbilityEffectPreset DashEffect = new AbilityEffectPreset("Dash effect", 5, 0.2f, _id: 7);
        public static AbilityEffectPreset dmgBonusEffect = new AbilityEffectPreset("BonusIfDoT Effect", 0, 8.0f, 0.0f, 0.0f, _hitAction: dotDmgBonus, _id: 8);
        public static AbilityEffectPreset BuffNextTB = new AbilityEffectPreset("Buff next TB", 0, 0.0f, 10.0f, _startAction:buffNextTB_add, _finishAction: buffNextTB_remove, _id: 9);
        */
        public static AbilityEff shadowBoltEffect;
        public static AbilityEff mindBlastEffect;
        public static AbilityEff autoAttackEffect;
        public static AbilityEff igniteEffect;
        public static AbilityEff genericMagicEffect;
        public static Buff Doom;
        public List<AbilityEff> effectsList;
        
        public void OnValidate(){
            
            instance = this;

            Debug.Log("AED: Setting IDs of effects in effectsList");
            setIDs();

        }
        
        public void setIDs(){
            if(effectsList ==null){
                Debug.Log("AED has null effectsList");
            }
            if(effectsList.Count > 0){
                for (int i = 0; i < effectsList.Count; i++)
                {
                    effectsList[i].id = i;
                }
            }
        }
        public AbilityEff find(int _id){
            foreach(AbilityEff effect in effectsList){
                if(effect.id == _id){
                    return effect;
                }
            }
            return null;
    }
        
/*
        ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~vV*****Start/ Hit/ Finish Actions*****Vv~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
*/
    /* OLD
        public static void secondaryTestboltFinish(AbilityEffect _ae){
            _ae.getCaster().freeCast(PlayerAbilityData.CastedDamage, _ae.getTarget());
        }
        public static void secondaryDoT(AbilityEffect _ae){
            _ae.getCaster().freeCast(PlayerAbilityData.DoT, _ae.getTarget());
        }
        
        public static void dotDmgBonus(AbilityEffect _ae){
            //if(  target has DoT debuff  )

            if(_ae.getTarget().getActiveEffects().Find(x => x.getID() == AbilityEffectData.DelayedOneOffEffect.getID()) != null){
                Debug.Log("BOOM double damage!");
                _ae.getTarget().damageValue( (int) (_ae.getPower() * 2));
            }
            else{
                Debug.Log("No required buff. Normal Damage");
                standardDamage(_ae);
            }
            
        }
        public static void standardDamage(AbilityEffect _ae){
            _ae.getTarget().damageValue( (int) _ae.getPower() );
        }
        public static void standardRestore(AbilityEffect _ae){
            _ae.getTarget().restoreValue( (int) _ae.getPower() );
        }
        public static void buffNextTB_add(AbilityEffect _ae){
            _ae.getCaster().aeFireEvent.AddListener(buffTBInList);
        }
        public static void buffTBInList(List<AbilityEffect> _outGoingEffects){
            for(int i = 0; i < _outGoingEffects.Count; i++){
                if(_outGoingEffects[i].getID() == AbilityEffectData.oneOffDamageEffect.getID()){
                    _outGoingEffects[i].setPower(_outGoingEffects[i].getPower() * 4);
                    _outGoingEffects[i].getCaster().RemoveActiveEffect(x => x.getID() == AbilityEffectData.BuffNextTB.getID());
                    // ^^^ problem need a way of removing an active effect from an actor
                    //     with proferably either a ref to the obj or a pos
                }
            }
        }
        public static void buffNextTB_remove(AbilityEffect _ae){
            _ae.getCaster().aeFireEvent.RemoveListener(buffTBInList);
        }*/
        
        /*public static void exampleHookIn(Actor caster, ref Ability AbilityToCast){
            
        }*/
}
