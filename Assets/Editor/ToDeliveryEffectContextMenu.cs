using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


 [InitializeOnLoad]
public class ToDeliveryEffectContextMenu
 {
     static ToDeliveryEffectContextMenu()
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
            var field = targetObjectClassType.GetField("eInstructs");
            
            
            if (field != null)
            {
                var value = field.GetValue(targetObject);
                if(value.GetType() == typeof(List<EffectInstruction>)){
                    // Now would be where you would search the lst at the index you got from
                    // from property.propertyPath but... for now hardcode.
                    List<EffectInstruction> eiList = (List<EffectInstruction>)value;

                    
                    menu.AddItem(new GUIContent("To Delievery Effect"), false, () =>
                                {
                                    ToDeliveryEffectWindow window = EditorWindow.GetWindow<ToDeliveryEffectWindow>();
                                    window.selectedAbility = ability;
                                    window.Show();
                                });
                        

                    // if(eiList[0] != null){
                    //     if(eiList[0].effect.GetType() == typeof(Aoe)){
                    //         menu.AddItem(new GUIContent("We Got It! kinda.."), false, () =>
                    //             {
                    //                 Debug.Log("Break this Aoe!");
                    //                 Undo.RecordObject(ability, "Break Delivery Effect");
                    //                 List<EffectInstruction> listCopy = new List<EffectInstruction>((eiList[0].effect as Aoe).eInstructs);
                    //                 int insertCount = 1; //Element posistion + 1
                    //                 foreach(EffectInstruction ei in listCopy){
                    //                     ability.eInstructs.Insert(insertCount, ei);
                    //                     insertCount++;
                    //                 }
                    //                 ability.eInstructs.RemoveAt(0); // "Element posistion from path"
                    //             });
                    //     }
                    // }
                    
                }
                else{
                    Debug.Log("No " + typeof(List<EffectInstruction>).ToString() + " in Ability_V2: " + ability.getName());
                }

            }
            else{
                Debug.Log("Field " + property.propertyPath + " not found in Ability_V2: " + ability.getName());
            }
        }
    }
    
 }
