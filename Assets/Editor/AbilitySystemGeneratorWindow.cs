using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
public class AbilitySystemGeneratorWindow : EditorWindow
{
   private string abilityName = "";
   private string abilityEffectName = "";
   int effectSelection;
   string[] abilityEffectTypes = new string[]{nameof(SchoolDamage), nameof(Missile), nameof(Aoe), nameof(RingAoe), nameof(ApplyBuff), nameof(Dizzy), nameof(Heal), nameof(RestoreClassResource)};

   bool createWithAbility = false;
   bool addToDatabases = false;
    [MenuItem("Ability/Ability System Tools")]
    public static void ShowWindow()
    {
        GetWindow(typeof(AbilitySystemGeneratorWindow));
    }

    private void OnGUI()
    {
         GUILayout.Label("Ability System Tools", EditorStyles.boldLabel);
         abilityEffectName = EditorGUILayout.TextField("Effect Name", abilityEffectName);
         abilityName = EditorGUILayout.TextField("Ability Name", abilityName);
         
        
        effectSelection = EditorGUILayout.Popup(effectSelection, abilityEffectTypes);
        addToDatabases = EditorGUILayout.Toggle("Add To Databases", addToDatabases);
        if (GUILayout.Button("Build Effect")){
         // abilityEffectName = EditorGUILayout.TextField("Effect Name", abilityEffectName);
            GenerateSOs();
            
        }
        if (GUILayout.Button("Generate Ability With Effect")){
            createWithAbility = true;
            GenerateSOs();
        }
        
        EditorGUILayout.HelpBox(
         "\n> ~~~~~ ~~~~~WARNING!~~~~~ ~~~~~ <\n\n This does not yet add created Abilities or effects into AbilityDatabase or AbilityEffectDatabase!",
                      MessageType.Warning); 
    }

    private void GenerateSOs()
    {
        if (!Directory.Exists($"Assets/Scripts/Ability/AbilityEff/" + abilityEffectName + ".asset")){
            AbilityEff aeRef = null;
            //{"Magic Damage", "Missile", "AoE", "Ring Aoe", "Apply Buff", "Dizzy", "Heal"}
            switch(effectSelection){
               case(0):
                  aeRef = CreateInstance(typeof(SchoolDamage)) as AbilityEff;
                  
                  break;
               case(1):
                  aeRef = CreateInstance(typeof(Missile)) as AbilityEff;
                  
                  break;
               case(2):
                  aeRef = CreateInstance(typeof(Aoe)) as AbilityEff;
                  
                  break;
               case(3):
                  aeRef = CreateInstance(typeof(RingAoe)) as AbilityEff;
                  
                  break;
               case(4):
                  aeRef = CreateInstance(typeof(ApplyBuff)) as AbilityEff;
                  
                  break;
               case(5):
                  aeRef = CreateInstance(typeof(Dizzy)) as AbilityEff;
                  
                  break;
               case(6):
                  aeRef = CreateInstance(typeof(Heal)) as AbilityEff;
                  
                  break;
               case(7):
                  aeRef = CreateInstance(typeof(RestoreClassResource)) as AbilityEff;

                  break;
               default:
                  Debug.LogError("Unknown selection");
                  break;
            }
            aeRef.effectName = abilityEffectName;
            AssetDatabase.CreateAsset(aeRef, "Assets/Scripts/Ability/AbilityEff/" + abilityEffectName + ".asset");
            
            Debug.Log(aeRef.GetType().ToString() + "| Assets/Scripts/Ability/AbilityEff/" + abilityEffectName + ".asset");
            if(addToDatabases){
               addEffectToDatabase(aeRef);
            }
            // Directory.CreateDirectory($"Assets/{abilityName}");
            if(createWithAbility){
               Ability_V2 abilityRef;
               abilityRef = CreateInstance(typeof(Ability_V2)) as Ability_V2;
               abilityRef.setName(abilityName);
               List<EffectInstruction> temp = new List<EffectInstruction>();
               temp.Add(new EffectInstruction(aeRef, 0));
               abilityRef.setEffectInstructions(temp);
               AssetDatabase.CreateAsset(abilityRef, "Assets/Scripts/Ability/" + abilityName + ".asset");
               Debug.Log(abilityRef.GetType().ToString() + "| Assets/Scripts/Ability/" + abilityName + ".asset");
               if(addToDatabases){
                  addAbilityToDatabase(abilityRef);
               }
               

               createWithAbility = false;
               temp = null;

            }
        }
        else{
            Debug.LogError("Ability already exists with that name");
        }
            
      	void addEffectToDatabase(AbilityEff _effect){
            AbilityEffectData.instance.effectsList.Add(_effect);
         }
         void addAbilityToDatabase(Ability_V2 _ability){
            AbilityData.instance.abilityList.Add(_ability);
         }
      //   foreach (GameObject go in Selection.gameObjects)
      //   {
      //       string localPath = AssetDatabase.GenerateUniqueAssetPath($"Assets/{pathName}{go.name}.prefab");
      //       PrefabUtility.SaveAsPrefabAssetAndConnect(go, localPath, InteractionMode.UserAction);
      //   }    
    }
}
