using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CastBar : MonoBehaviour
{
    public Text abilityName;
    public Image castFill; //  Used to make fill bar different color
    public Slider castBar;
    public float castTime;
    public float elaspedTime = 0.0f;
    public Actor caster;
    public Actor target;
    public bool start = false;

    public AbilityEffect AbilityEffect;
    
  void Start(){

  }
  public void Init(AbilityEffect ability_effect, string ability_name, Actor from_caster, Actor to_target, float cast_time){
    AbilityEffect = ability_effect;
    abilityName.text = ability_name;
    caster = from_caster;
    target = to_target;
    castTime = cast_time;
    castBar.maxValue = cast_time;

    elaspedTime = 0.0f;
    start = true;
  }
    void Update(){
      if(start){
        if(elaspedTime < castTime){
          castBar.value = elaspedTime;
          elaspedTime += Time.deltaTime;
        }
        else{
          Debug.Log("SC: cast Completed!");

          // adding AbilityEffect to target actor's abilityEffects list
          target.applyAbilityEffect(AbilityEffect, caster);
          
          Destroy(gameObject);
          //target.health -= 15.0f;
        }
        
      }
    }
}
