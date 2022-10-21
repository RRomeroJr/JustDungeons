using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
public class ToDeliveryEffectWindow : EditorWindow
{
   public Ability_V2 selectedAbility;
   public string abilityEffectName;
   public int lowerRange;
   public int upperRange; 
   int effectSelection;
   string[] abilityEffectTypes = new string[]{ "Missile", "AoE", "Ring Aoe", "Apply Buff"};

   DeliveryEff aeRef;
   
   
    

    private void OnGUI()
    {
         GUILayout.Label("To Delivery Effect", EditorStyles.boldLabel);
         selectedAbility = EditorGUILayout.ObjectField("Selected Ability Name", selectedAbility, typeof(Ability_V2), false) as Ability_V2;
         lowerRange = EditorGUILayout.IntField("Lower range", lowerRange); //In future make this start with the index clicked
         upperRange = EditorGUILayout.IntField("Upper range", upperRange);
         effectSelection = EditorGUILayout.Popup(effectSelection, abilityEffectTypes);
         abilityEffectName = EditorGUILayout.TextField("Effect Name", abilityEffectName);
         if (GUILayout.Button("Combine to Delivery Effect")){
         // abilityEffectName = EditorGUILayout.TextField("Effect Name", abilityEffectName);
            GenerateSOs();
            
        }
       EditorGUILayout.HelpBox(
         "\n> ~~~~~ ~~~~~WARNING!~~~~~ ~~~~~ <\n\n if you undo this action the created DeliveryEffect will remain! You will have to delete it manually to get rid of it.",
                      MessageType.Warning); 

    }
   private void GenerateSOs()
    {
      Undo.RecordObject(selectedAbility, "Add to delievery effect");
        if (selectedAbility != null){
            
            switch(effectSelection){
               case(0):
                  
                  aeRef = CreateInstance(typeof(Missile)) as Missile;
                  Debug.Log("Build Missile");
                  break;
               case(1):
                  
                  aeRef = CreateInstance(typeof(Aoe)) as Aoe;
                  Debug.Log("Build Aoe");
                  break;
               case(2):
                  
                  aeRef = CreateInstance(typeof(RingAoe)) as RingAoe;
                  Debug.Log("Build RingAoe");
                  break;
               case(3):
                  
                  aeRef = CreateInstance(typeof(ApplyBuff)) as ApplyBuff;
                  Debug.Log("Build ApplyBuff");
                  break;
               default:
                  Debug.LogError("Unknown selection");
                  break;
            }
            // aeRef.effectName = abilityEffectName;
            List<EffectInstruction> eiList = new List<EffectInstruction>();
            for(int i=lowerRange;i<=upperRange; i++){
               eiList.Add(selectedAbility.eInstructs[i].cloneNoEffectClone());
            }
            
            
            aeRef.eInstructs = new List<EffectInstruction>();
            foreach (EffectInstruction ei in eiList){
               aeRef.eInstructs.Add(ei);
            }
            aeRef.effectName = abilityEffectName;
            
            AssetDatabase.CreateAsset(aeRef, "Assets/Scripts/Ability/AbilityEff/" + abilityEffectName + ".asset");
            
            EffectInstruction eiContainer = new EffectInstruction();
            eiContainer.effect = aeRef;

            selectedAbility.eInstructs.Insert(lowerRange, eiContainer);
            for(int i = (lowerRange  + 1);i<= (upperRange + 1); i++){
               //Debug.Log(selectedAbility.eInstructs[lowerRange  + 1].effect.effectName + " removing.. i =" + i.ToString());
               selectedAbility.eInstructs.RemoveAt(lowerRange  + 1);
            }
            //Undo.RegisterCreatedObjectUndo(aeRef, "EIs to delivery effect"); not working
            
        }
        else{
            Debug.LogError("Ability already exists with that name");
        }
            

      //   foreach (GameObject go in Selection.gameObjects)
      //   {
      //       string localPath = AssetDatabase.GenerateUniqueAssetPath($"Assets/{pathName}{go.name}.prefab");
      //       PrefabUtility.SaveAsPrefabAssetAndConnect(go, localPath, InteractionMode.UserAction);
      //   }    
    }
    
    
}
