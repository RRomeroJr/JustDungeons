using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEditor;


 [InitializeOnLoad]
public class eiListEditor
 {
     static eiListEditor()
     {
         EditorApplication.contextualPropertyMenu += OnContextMenuOpening;
     }
 
     private static void OnContextMenuOpening(GenericMenu menu, SerializedProperty property)
     {
        var targetObject = property.serializedObject.targetObject;
        var targetObjectClassType = targetObject.GetType();
        if(targetObjectClassType== typeof(Ability_V2)){
            Ability_V2 ability = (Ability_V2)targetObject;
            
            //eInstructs.Array.data[0] need to get rid of the .Array.data[0]
            //Also see if eInstructs[0] works
            // For now hard code "eInstructs" field name
            string arrayExtension = ".Array.data[_]";
            string pathWithoutArrayExtension = property.propertyPath;

            pathWithoutArrayExtension =
                pathWithoutArrayExtension.Remove
                ((pathWithoutArrayExtension.Length) - arrayExtension.Length, arrayExtension.Length);
            //Debug.Log(pathWithoutArrayExtension);
            var field = targetObjectClassType.GetField(pathWithoutArrayExtension);
            
            
            if (field != null) //if there is a field called "eInstructs"
            {
                var value = field.GetValue(targetObject);
                if(value.GetType() == typeof(List<EffectInstruction>)){
                    // Now would be where you would search the lst at the index you got from
                    // from property.propertyPath but... for now hardcode.
                    List<EffectInstruction> abilityEIList = (List<EffectInstruction>)value;





                    string temp = property.propertyPath; // eInstructs.Array.data[1]

                    char temp2 = temp[temp.Length - 2];

                    if(Char.IsNumber(temp2)){
                        
                        int index = (int)Char.GetNumericValue(temp2);
                        Debug.Log("eiList.count: " + abilityEIList.Count.ToString() +
                                                 " | eiList[" + index.ToString() + "]");
                        if(abilityEIList[index] != null){
                            if(abilityEIList[index].effect.GetType().IsSubclassOf( typeof(DeliveryEff))){
                                menu.AddItem(new GUIContent("Break Delivery Effect"), false, () =>
                                {
                                    DeliveryEff deAtIndex = abilityEIList[index].effect as DeliveryEff;
                                    // Debug.Log("Break this Delivery Effect!");
                                    Undo.RecordObject(ability, "Break Delivery Effect");
                                    List<EffectInstruction> deListCopy = deAtIndex.eInstructs.cloneInstructsNoEffectClone();
                                    
                                    int insertCount = index + 1; //Element posistion + 1
                                    foreach(EffectInstruction ei in deListCopy){
                                        Debug.Log(ei.effect.GetType().ToString());
                                        ability.eInstructs.Insert(insertCount, ei);
                                        insertCount++;
                                    }
                                    ability.eInstructs.RemoveAt(index); // "Element posistion from path"
                                });
                            }
                        }
                    
                    
                    }
                
                }
                else{
                    Debug.Log("No " + typeof(List<EffectInstruction>).ToString() + " in Ability_V2: " + ability.getName());
                }

            }
            else{
                Debug.Log("Field " + property.propertyPath + " not found in Ability_V2: " + ability.getName());
            }
        }

        // if(property.type == typeof(EffectInstruction).ToString()){
        //     if(property.isArray == false){
                
        //         menu.AddItem(new GUIContent("We Got It! kinda.."), false, () =>
        //         {
        //             Debug.Log("Break this Aoe!");
        //         });
        //     }
        
        
         Debug.Log("ContextMenu opening for property " + property.propertyPath);
         if(property.isArray == true){
            Debug.Log(property.type + " property is an array sT: " + property.propertyType.ToString());
         }else{
            Debug.Log(property.type + " property NOT array sT: " + property.propertyType.ToString());
         } 
    }
    void weGotIt(){
        Debug.Log("We found an EffectInstruction!");
    }
 }
