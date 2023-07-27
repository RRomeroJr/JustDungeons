using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;
using System;

public class InGameMenu : MonoBehaviour
{
  void Update()
  {
    if(Input.GetKeyDown("escape"))
    {
      
    }
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
}