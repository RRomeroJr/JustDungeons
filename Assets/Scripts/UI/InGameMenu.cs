using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;
using System;
using UnityEngine.UI;

public class InGameMenu : MonoBehaviour
{
  #region Options
  public GameObject SFXSlider;
  #endregion
  void OnEnable()
  {
    UIManager.Instance.blockCameraControls = true;
    SFXSlider.GetComponent<Slider>().value = AudioManager.instance.SFXVol;
  }
  void OnDisable()
  {
    UIManager.Instance.blockCameraControls = false;
  }
  public void ExitGame()
  {
  // if(NetworkServer.active)
  // {
  //   Debug.Log("You are the host. close game to quit");
  //   return;
  // }
  if(NetworkServer.active)
  {
    NetworkManager.singleton.StopHost();
  }
  else
  {
    NetworkManager.singleton.StopClient();

  }
    SceneManager.LoadScene("TitleScene");
  }
  public void ChangeSFXVolume(float _val)
  {
      AudioManager.instance.SFXVol = _val;
  }
}