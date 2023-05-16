using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.AI;
using UnityEngine.UI;
using System;

public class UIBuff : MonoBehaviour
{
  private BuffSystem.Buff buff;
  public Slider buffSpinner;
  public Image buffIcon;
  
  void Update()
  {
    buffSpinner.value = buff.BuffSO.Duration - buff.RemainingBuffTime;
  }
    
  
  // void OnRemoveBuff()
  // {
  //   buff = null;
  //   buffSpinner = null;
 
  // }
  public void ClearBuff()
  {
    buff = null;
  }
  public void AddBuff(BuffSystem.Buff _newBuff)
  {
    buff = _newBuff;

    buffSpinner.maxValue = buff.BuffSO.Duration;
    buffSpinner.value = buff.RemainingBuffTime;
    buffIcon.sprite = buff.BuffSO.Icon;
  }
  
}

