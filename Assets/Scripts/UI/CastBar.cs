using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CastBar : MonoBehaviour
{
    public Text castName;
    public Image castFill; //  Used to make fill bar different color
    public Slider castBar;
    public float castTime;
    public float elaspedTime = 0.0f;
    public AbilityHandler caster;
    public Actor targetActor;
    public Vector3 targetWP;
    public Vector3 target;
    public Color interruptableColor;
    public Color uninterruptableColor;
    public bool start = false;
    
  void Start(){

  }
  
  public void Init(string cast_name, AbilityHandler from_caster, Actor to_targetActor, float cast_time){
    castName.text = cast_name;
    caster = from_caster;
    targetActor = to_targetActor;
    castTime = cast_time;
    castBar.maxValue = cast_time;

    elaspedTime = 0.0f;
    start = true;
  }public void Init(string cast_name, AbilityHandler from_caster, Vector3 to_targetWP, float cast_time){

    castName.text = cast_name;
    caster = from_caster;
    targetWP = to_targetWP;
    castTime = cast_time;
    castBar.maxValue = cast_time;

    elaspedTime = 0.0f;
    start = true;
  }
  public void OnAbilityChanged()
  {
    castName.text = caster.QueuedAbility.getName();
    if(caster.QueuedAbility.isChannel)
    {
      castBar.maxValue = caster.QueuedAbility.channelDuration;
    }
    else
    {
      castBar.maxValue = caster.QueuedAbility.getCastTime();;
    }
    // castTime = caster.QueuedAbility.getCastTime();

  }
    void Update(){
      
      if(caster.IsCasting)
      {
        if(caster.QueuedAbility.interruptable && castFill.color != interruptableColor)
        {
          castFill.color = interruptableColor;
        }
        else if(!caster.QueuedAbility.interruptable && castFill.color != uninterruptableColor)
        {
          castFill.color = uninterruptableColor;
        }
        castBar.value = caster.castTime;
      }
      else
      {
        gameObject.active = false;
      }
      
    }

    void OnDestroy(){
      
    }
}
