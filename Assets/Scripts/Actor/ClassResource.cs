using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class ClassResource
{
   public ClassResourceType crType;
  public int max;
  public int amount;
  public int combatRegen;
  public int outOfCombatRegen;

  public ClassResource Copy(){
    ClassResource toReturn = new ClassResource();
    toReturn.crType = crType;
    toReturn.max = max;
    toReturn.amount = amount;
    toReturn.combatRegen = combatRegen;
    toReturn.outOfCombatRegen = outOfCombatRegen;
    return toReturn;
  }
 
}
