using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class DamageText : MonoBehaviour
{
  
   //public static GameObject prefab;
   public float duration = 0.75f;
   public static GameObject Create(Vector2 pos, int amount){
     

    var gmObj = Instantiate(UIManager.damageTextPrefab, pos, Quaternion.identity) as GameObject;
    gmObj.GetComponent<TextMeshPro>().SetText(amount.ToString());
    return gmObj;
   }
   void Update(){
    if(duration <= 0){
        Destroy(gameObject);
    }
    duration -= Time.deltaTime;
    
   }
   
}
