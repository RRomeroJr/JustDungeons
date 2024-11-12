using System;
using System.Collections.Generic;
using BuffSystem;
using Mirror;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Video;

#if UNITY_EDITOR
using UnityEditor.Callbacks;
#endif

/// <summary>
/// General purpose buff container which implements every buff in the game
/// </summary>
public class BuffHandler_V2 : NetworkBehaviour
{
    public List<OldBuff.Buff> buffList = new List<OldBuff.Buff>();
    public UnityEvent OnBuffUpdate = new UnityEvent();
    #region EventRaised
    #endregion
    public void AddBuff(OldBuff.Buff _buffClone)
    {
        Debug.Log("AddBuff");
        var bef = buffList.Count;
        OnBuffUpdate.AddListener(_buffClone.update);
        buffList.Add(_buffClone);
        Debug.Log(_buffClone.name + "added");
        RpcAddBuff(_buffClone);

        var aft = buffList.Count;
        if(bef == aft){
            Debug.LogError("before and after AddBuff are the same Count");
        }
        else
        {
            Debug.Log(string.Format("A buff was added you are not crazy: {0} => {1}", bef, aft));
        }
        if(_buffClone.remainingTime <= 0){
            Debug.Log(string.Format("Hey, the {0} you added has a remainingTime of <= 0. Will end rright way", _buffClone.name));
        }
        else{
            Debug.Log(string.Format("{0} remainingTime is > 0, good", _buffClone.name));
        }
    }
    [ClientRpc]
    public void RpcAddBuff(OldBuff.Buff _buffFromServer){
        if(isServer){
            return;
        }
        OnBuffUpdate.AddListener(_buffFromServer.update);
        buffList.Add(_buffFromServer);
        Debug.Log("RpcAddBuff" + _buffFromServer.name + "added");
    }
    void Update(){
        OnBuffUpdate.Invoke();
    }
    
    [Server]
    public void RemoveBuff(OldBuff.Buff _callingBuff)
    {
        int buffIndex = buffList.FindIndex(x => x == _callingBuff);

        // buffList.RemoveAt(buffIndex);
        Debug.Log("Rpc-ing to remove index: " + buffIndex);

        RpcRemoveBuffIndex(buffIndex);
    }

    [ClientRpc]
    void RpcRemoveBuffIndex(int hostIndex)
    {
        Debug.Log("Host saying to remove buff index: " + hostIndex);
        buffList[hostIndex].OnRemoveFromList();
        // var before = OnBuffUpdate.GetPersistentEventCount();
        var buffRef = buffList[hostIndex];
        OnBuffUpdate.RemoveListener(buffRef.update);
        // var after = OnBuffUpdate.GetPersistentEventCount();
        // Debug.Log("before: " + before +  "after: " + after);
        buffList.RemoveAt(hostIndex);
        Destroy(buffRef);

    }


}
