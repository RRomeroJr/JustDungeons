using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using UnityEditor.SceneManagement;
public class Hotbutton: MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public Canvas canvas;

    public Transform parentSlot;
    public Image image;
    public Ability_V2 ability;
    public Slider cooldownSlider;
   
    private AbilityCooldown abilityCooldown;
    public int cooldownIndex = -1;
    public TextMeshProUGUI tempAbilityNameText;

    //public DebugTimer debugTimer = new DebugTimer(2.0f);

    void OnValidate(){
        if(tempAbilityNameText == null){
            return;
        }
        if(ability == null){
            return;
        }
        Debug.Log("Setting ability text");
        tempAbilityNameText.SetText(ability.getName());
        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
     }
    void Awake(){
        image = GetComponent<Image>();
        UIManager.removeCooldownEvent.AddListener(OnCooldownRemoved);
        
        abilityCooldown = null;

    }
    void OnCooldownRemoved(int _removedIndex){
        if(_removedIndex == cooldownIndex){
            // Debug.Log("Event nulling abilityCooldown");
            abilityCooldown = null;
            cooldownIndex = -1;
            
        }
    }
    public void OnBeginDrag(PointerEventData eventData){
        parentSlot = null;
        transform.SetParent(canvas.transform);
        transform.SetAsLastSibling();
        if(image != null){
            image.raycastTarget = false;
        }
        if(tempAbilityNameText != null){
            tempAbilityNameText.raycastTarget = false;
        }
    }
    public void OnDrag(PointerEventData eventData){
        //rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        transform.position = Input.mousePosition;
    }
    public void OnEndDrag(PointerEventData eventData){
        if(parentSlot != null){
            transform.SetParent(parentSlot);
            transform.SetAsFirstSibling();
            
        }
        if(image != null){
            image.raycastTarget = true;
        }
        
        if(tempAbilityNameText != null){
            tempAbilityNameText.raycastTarget = true;
        }
       
         
        
        
    }
 
    string checkAbilityCooldown(){
        if(abilityCooldown == null){
            return "abilityCooldown is empty";
        }
        else{
            return "abilityCooldown is filled";
        }
    }
    void Update(){
        
        if(abilityCooldown == null){
            if(UIManager.playerActor.checkOnCooldown(ability)){
                cooldownIndex = UIManager.playerActor.abilityCooldowns.FindIndex(x => x.abilityName == ability.getName());
                abilityCooldown = UIManager.playerActor.abilityCooldowns[cooldownIndex];
                // Debug.Log("abilityCooldown set");
                cooldownSlider.maxValue = ability.getCooldown();
                cooldownSlider.value = abilityCooldown.remainingTime;

            }
        }
        else{
            if(abilityCooldown.remainingTime > 0.0f ){
                cooldownSlider.value = abilityCooldown.remainingTime;
            }
            
        }
        
    }
    public void SetUp(){
        if(ability == null){
            Debug.LogError(name + " Hotbutton set up failed. No ability. Destroying");
            Destroy(gameObject);
            return;
        }
        tempAbilityNameText.SetText(ability.getName());
        //Set up background image & color here
    }

}