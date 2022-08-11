using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CastBarNPC : MonoBehaviour
{
    /*public Text castName;
    public Image castFill; //  Used to make fill bar different color
    public Slider castBar;*/
    public float castTime;
    public float elaspedTime = 0.0f;
    public Actor caster;
    public Actor targetOLD;
    public Vector3 target;
    public bool start = false;
    
  void Start(){

  }
  public void Init(string cast_name, Actor from_caster, Actor to_target, float cast_time){
    Init(cast_name, from_caster, to_target.gameObject.transform.position, cast_time);
  }
  public void Init(string cast_name, Actor from_caster, Vector3 to_target, float cast_time){

    //castName.text = cast_name;
    caster = from_caster;
    target = to_target;
    castTime = cast_time;
    //castBar.maxValue = cast_time;

    elaspedTime = 0.0f;
    start = true;
  }
    void Update(){
      if(start){
        if(elaspedTime < castTime){
          //castBar.value = elaspedTime;
          elaspedTime += Time.deltaTime;
        }
        else{
          //Debug.Log("CBarNPC: cast Completed!");

          //  Check if NPC can see target?
          
          //Signaling back to Actor that cast completed
          caster.readyToFire = true;
          Destroy(this);
        }
        
      }
    }
    void OnDestroy(){
      //Signaling back to Actor that no longer casting
      caster.isCasting = false;
    }
}
