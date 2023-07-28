using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class TransportSwitcher : MonoBehaviour
{
    public void SwitchToKCP()
    {
        try
        {
            Destroy(GetComponent<Mirror.FizzySteam.FizzySteamworks>());
            // Destroy(GetComponent<SteamManager>());
        }
        catch{}

        kcp2k.KcpTransport kcpTrans;

        if(TryGetComponent<kcp2k.KcpTransport>(out kcp2k.KcpTransport existingKcp))
        {
            kcpTrans = existingKcp;
        }
        else
        {
            kcpTrans = gameObject.AddComponent<kcp2k.KcpTransport>();
        }

        CustomNetworkManager.singleton.transport = kcpTrans;
        Transport.active = kcpTrans;
        Debug.Log("KcpTransport Active");
    }
    public void SwitchToSteam()
    {
        try
        {
            Destroy(GetComponent<kcp2k.KcpTransport>());
        }
        catch{}

        Mirror.FizzySteam.FizzySteamworks _fizzySteam;
        // SteamManager _steamManager;

        // if(TryGetComponent<SteamManager>(out SteamManager existingSteamManager))
        // {
        //     _steamManager = existingSteamManager;
        // }
        // else
        // {
        //     _steamManager = gameObject.AddComponent<SteamManager>();
        // }
        if(TryGetComponent<Mirror.FizzySteam.FizzySteamworks>(out Mirror.FizzySteam.FizzySteamworks existingFizzySteam))
        {
            _fizzySteam = existingFizzySteam;
        }
        else
        {
            _fizzySteam = gameObject.AddComponent<Mirror.FizzySteam.FizzySteamworks>();
        }

        CustomNetworkManager.singleton.transport = _fizzySteam;
        Transport.active = _fizzySteam;
        Debug.Log("FizzySteamworks Active");
    }
    void Update()
    {
        if(Input.GetKeyDown("f7"))
        {
            if(NetworkServer.active || NetworkClient.active)
            {
                Debug.LogError("You cannot switch transports while Server or Client is active");
            }
            else
            {
                SwitchToKCP();
            }
        }
        if(Input.GetKeyDown("f8"))
        {
            if(NetworkServer.active || NetworkClient.active)
            {
                Debug.LogError("You cannot switch transports while Server or Client is active");
            }
            else
            {
                SwitchToSteam();
            }
        }
    }
    void Start()
    {
        Debug.Log("F7 to switch to switch KcpTransport");
        Debug.Log("F8 to switch to FizzySteam");

    }
}
