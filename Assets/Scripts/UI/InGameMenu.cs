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
  #endregion
  void OnEnable()
  {
    UIManager.Instance.blockCameraControls = true;
    if (AudioManager.instance == null)
    {
      MsgBox.DisplayMsg("Error reading game settings");
      return;
    }
    transform.Find("OptionsMenu/SFX_Vol/Slider").GetComponent<Slider>().value = AudioManager.instance.SFXVol;
    transform.Find("OptionsMenu/Music_Vol/Slider").GetComponent<Slider>().value = AudioManager.instance.MusicVol;
    transform.Find("OptionsMenu/UI_Vol/Slider").GetComponent<Slider>().value = AudioManager.instance.UIVol;
    
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
  public void ChangeMusicVolume(float _val)
  {
      AudioManager.instance.MusicVol = _val;
  }
  public void ChangeUIVolume(float _val)
  {
      AudioManager.instance.UIVol = _val;
  }
}