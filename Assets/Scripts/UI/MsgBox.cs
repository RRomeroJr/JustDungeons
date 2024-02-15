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
}