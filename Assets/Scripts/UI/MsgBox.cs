using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class MsgBox : MonoBehaviour
{
  public List<MsgBoxItem> mbiList = new List<MsgBoxItem>();
  public static MsgBox instance;

  void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

  public static void DisplayMsg(string _in)
  {
    for(int i = instance.mbiList.Count - 1; i > 0; i--)
    {
    if(instance.mbiList[i - 1].gameObject.activeSelf == false)
    {// if the one above is not active, skip
      continue;
    }

    instance.mbiList[i].StartDisplaying(instance.mbiList[i - 1].tmpUi.text, instance.mbiList[i - 1].timer);
    
    }


    instance.mbiList[0].StartDisplaying(_in);

  }
  public static void CantDoThatRightNow()
  {
    DisplayMsg("You can't do that right now.");
  }
  public static void NotReadyYet()
  {
    DisplayMsg("That isn't ready yet.");
  }
  public static void NotInRange()
  {
    DisplayMsg("You're not in range.");
  }
  public static void OnCooldown()
  {
    DisplayMsg("That is on cooldown.");
  }
  public static void NotEnoughResources()
  {
    DisplayMsg("Not enough resources.");
  }
  public static void NeedTarget()
  {
    DisplayMsg("You lack a target.");
  }
  public static void NotFacing()
  {
    DisplayMsg("You must face your target.");
  }
}