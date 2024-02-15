using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Mirror;
using Steamworks;

public class WebScript : MonoBehaviour
{
    void Update()
    {
        if(Input.GetKeyDown("]"))
        {
            StartCoroutine(AddPlayer());
        }
    }
    IEnumerator AddPlayer()
    {
        WWWForm form = new WWWForm();
        form.AddField("steamid", SteamUser.GetSteamID().m_SteamID.ToString());

        using(UnityWebRequest www = UnityWebRequest.Post("http://localhost/JustDungeons/AddPlayer.php", form))
        {
            yield return www.SendWebRequest();

            if(www.isNetworkError || www.isHttpError)
            {
                Debug.LogError(www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
            }
        }
    }
}
