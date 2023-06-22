using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
/*
    Container with references important to controlling
    and displaying unit frames properly
*/
public class Nameplate : MonoBehaviour
{
    const float unslectedScale = 0.01f;
    const float selectedScale = 0.0125f;
    public Text unitName;
    public Image healthFill;
    public Image resourceFill;
    public Slider healthBar;
    public Slider resourceBar;
    public Slider castBar;
    public Text castName;
    public Actor actor;
    public Vector2 offset;
    public Canvas canvas;
    private Renderer actorRenderer;
    public UnityEvent<bool> selectedEvent = new UnityEvent<bool>();
    void Awake(){
        offset = new Vector2(0f, 1.5f);
    }
    void Start(){
       healthBar =  transform.GetChild(1).GetComponent<Slider>();
       resourceBar =  transform.GetChild(2).GetComponent<Slider>();
       castBar =  transform.GetChild(3).GetComponent<Slider>();
       castName =  transform.GetChild(3).GetComponentInChildren<Text>();
       unitName.text = actor.ActorName;
       canvas = GetComponentInParent<Canvas>();
       actorRenderer = actor.GetComponent<Renderer>();
       selectedEvent.AddListener(SetSelectedScale);
       
    }
    public static Nameplate Create(Actor _actor){
        Nameplate npRef = (Instantiate(UIManager.nameplatePrefab) as GameObject).GetComponentInChildren<Nameplate>();
        npRef.transform.position = _actor.transform.position + (Vector3)npRef.offset;
         npRef.actor = _actor;
         return npRef;
    }

    void Update(){
        if(actor == null || !actor.gameObject.active){
            Destroy(canvas.gameObject);
            return;
        }
        transform.position = actor.transform.position + (Vector3)offset;
        updateSliderHealth();
        updateSliderResource(resourceBar);
        updateSliderCastBar();
        if(actor != null){
            canvas.sortingOrder = actorRenderer.sortingOrder;
        }
        
    }
    void SetSelectedScale(bool _selected){
        if(_selected){
            canvas.gameObject.transform.localScale = new Vector3(selectedScale, selectedScale, 1);
        }
        else{
            canvas.gameObject.transform.localScale = new Vector3(unslectedScale, unslectedScale, 1);
        }
    }
    void updateSliderHealth(){
        healthBar.maxValue = actor.MaxHealth;
        healthBar.value = actor.Health;
    }
    void updateSliderResource(Slider silder){
        if(actor.ResourceTypeCount() > 0){
            silder.maxValue = actor.getResourceMax(0);
            silder.value = actor.getResourceAmount(0);
        }
        
    }
    void updateSliderCastBar(){
        if(actor.getQueuedAbility() == null){
            castBar.value = 0.0f;
            castName.text = "";
            return;
        }
        
        //Actor has a queued ability
        
        castBar.maxValue = actor.getQueuedAbility().getCastTime();
        castBar.value = actor.castTime;
        castName.text =actor.getQueuedAbility().getName();
    }
}
