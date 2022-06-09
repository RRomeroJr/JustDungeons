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
    public Actor target;
    public bool start = false;
    
  void Start(){

  }
  public void Init(string cast_name, Actor from_caster, Actor to_target, float cast_time){

    castName.text = cast_name;
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
          //Debug.Log("CBar: cast Completed!");

          //Check if player can see target?
          
          //Signaling back to PlayerControllerHBC that cast completed
          caster.gameObject.GetComponent<PlayerControllerHBC>().castReady = true;
          Destroy(gameObject);
        }
        
      }
    }
    void OnDestroy(){
      //Signaling back to PlayerControllerHBC that no longer casting
      caster.gameObject.GetComponent<PlayerControllerHBC>().isCasting = false;
    }
}
