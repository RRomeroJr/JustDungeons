using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellCast : MonoBehaviour
{
    public Text spellName;
    public Image castFill; //  Used to make fill bar different color
    public Slider castBar;
    public float castTime;
    public float elaspedTime = 0.0f;
    public Actor caster;
    public Actor target;
    public bool start = false;

    public SpellEffect spellEffect;
    
  void Start(){

  }
  public void Init(SpellEffect spell_effect, string spell_name, Actor from_caster, Actor to_target, float cast_time){
    spellEffect = spell_effect;
    spellName.text = spell_name;
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

          // adding SpellEffect to target actor's spellEffects list
          target.applySpellEffect(spellEffect, caster);
          
          Destroy(gameObject);
          //target.health -= 15.0f;
        }
        
      }
    }
}
