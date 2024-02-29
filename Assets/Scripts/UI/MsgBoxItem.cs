using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;


public class MsgBoxItem : MonoBehaviour
{
  const float CLEARTIMER = 2.4f;
  public TextMeshProUGUI tmpUi;
  public float timer = 0.0f;
  void Awake()
  {
    tmpUi = GetComponent<TextMeshProUGUI>();
  }
  void FixedUpdate()
  {
    if(timer > 0)
    {
      timer -= Time.fixedDeltaTime;
    }
    if(timer < 0)
    {
      // tmpUi.text = "";
      gameObject.SetActive(false);
    }
  }
  /// <summary>
  /// Sets text and restarts timer
  /// </summary>
  /// <param name="_in"></param>
  public void StartDisplaying(string _in, float _time = CLEARTIMER){
    if(!gameObject.activeSelf)
    {
      gameObject.SetActive(true);
    }
    tmpUi.text = _in;
    timer = _time;
  }
}