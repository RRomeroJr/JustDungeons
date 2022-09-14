using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public struct SetActorMsg : NetworkMessage
    {
        public Actor oldActor;
        public Actor newActor;
        public SetActorMsg(ref Actor _oldActor, Actor _newActor ){
            oldActor = _oldActor;
            newActor = _newActor;
        }
    }
public static class HBCNetTools
{   
    static HBCNetTools(){
        Debug.Log("HBCNetTools static constructor called");
        NetworkServer.RegisterHandler<SetActorMsg>(setActorServerHandler);
        NetworkClient.RegisterHandler<SetActorMsg>(setActorClientHandler);
    }
    
    public static void setActorServerHandler(NetworkConnectionToClient conn, SetActorMsg msg){
        Debug.Log("Server: Client Req Set Actor: " + (msg.newActor != null ? msg.newActor.getActorName() : "null"));
        
        msg.oldActor = msg.newActor;
    }
    public static void setActorClientHandler(SetActorMsg msg){
        Debug.Log("Client: Server telling to Set Actor: " + (msg.newActor != null ? msg.newActor.getActorName() : "null"));
        msg.oldActor = msg.newActor;
    }
    public static void setActorServer(ref Actor _oldActor, Actor _newActor){
        SetActorMsg msg = new SetActorMsg(ref _oldActor, _newActor);
        NetworkClient.Send(msg);
    }
    public static void setActorAllClients(ref Actor _oldActor, Actor _newActor){
        SetActorMsg msg = new SetActorMsg(ref _oldActor, _newActor);
        NetworkServer.SendToAll(msg);
    }

   /*
    [ClientRpc]
    public static void rpcSetActor(ref Actor _oldActor, Actor _newActor){
        SetActorMsg msg = new SetActorMsg(){
            oldActor = _oldActor,
            newActor = _newActor
        };
        
        Debug.Log("Actor set: " + (_oldActor != null ? _oldActor.getActorName() : "null"));
    }
    [Command]
    public static void cmdSetActor(ref Actor _oldActor, Actor _newActor){
        SetActorMsg msg = new SetActorMsg(){
            oldActor = _oldActor,
            newActor = _newActor
        };
        
        Debug.Log("Request: " + (_newActor != null ? _newActor.getActorName() : "null"));

        NetworkC
        rpcSetActor(ref _oldActor, _newActor);
    }*/


}
