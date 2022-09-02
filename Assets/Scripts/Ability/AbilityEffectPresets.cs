using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
[System.Serializable]
public class AbilityEffectPresets{
    
    public List<AbilityEffectPreset> aepList;
    public List<int> deliveryTypes;
    public AbilityEffectPreset this[int key]{
     get
     {
         return aepList[key];
     }
     set
     {
         aepList[key] = value;
     }
 }
    
}
