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
    public Actor caster;
    public Actor targetActor;
    public Vector3 targetWP;
    public Vector3 target;
    public bool start = false;
    
  void Start(){

  }
  public void Init(string cast_name, Actor from_caster, Actor to_targetActor, float cast_time){
    castName.text = cast_name;
    caster = from_caster;
    targetActor = to_targetActor;
    castTime = cast_time;
    castBar.maxValue = cast_time;

    elaspedTime = 0.0f;
    start = true;
  }public void Init(string cast_name, Actor from_caster, Vector3 to_targetWP, float cast_time){

    castName.text = cast_name;
    caster = from_caster;
    targetWP = to_targetWP;
    castTime = cast_time;
    castBar.maxValue = cast_time;

    elaspedTime = 0.0f;
    start = true;
  }
    void Update(){
      if(start){
        if(caster.isCasting){
          castBar.value = caster.castTime;
        }
        else{
          //Debug.Log("CBar: cast Completed!");

          //Check if player can see target?
          
          //Signaling back to Player's Actor that cast completed
          
          Destroy(gameObject);
        }
        
      }
    }
    void OnDestroy(){
      
    }
}
