using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MirrorTestTools : NetworkBehaviour
{
    public void ClientDebugLog(string _output){
        if(isServer)
            RpcDebugLog(_output);
   }
    [ClientRpc]
   void RpcDebugLog(string _output){
        
        Debug.Log(_output);
   }
   public void TargetClientDebugLog(NetworkConnection client, string _output){
        if(isServer)
            TRpcDebugLog(client, _output);
   }
   [TargetRpc]
   void TRpcDebugLog(NetworkConnection client, string _output){
        
        Debug.Log(_output);
   }
   public static MirrorTestTools _inst;
   void Start(){
    _inst = this;
   }
}
