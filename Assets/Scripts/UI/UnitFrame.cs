using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.UI;
/*
    Container with references important to controlling
    and displaying unit frames properly
*/
public class UnitFrame : MonoBehaviour
{
    public Text unitName;
    public Image healthFill;
    public Slider healthBar;
    public Actor actor;
    public Slider resourceBar;
    public UIBuff[] buffDisplays = new UIBuff[7];
    public UIBuff[] debuffDisplays = new UIBuff[7];
    int lastBuffCount = 0;
    // void Update()
    // {
    //     int currBuffCount = actor.GetComponent<BuffHandler>().Buffs.Count;
    //     if(currBuffCount != lastBuffCount){
    //         lastBuffCount = currBuffCount;
    //         Debug.Log("From in unitfframe itself");
    //         SetUpBuffsDisplays();
    //     }
    // }
    public void OnBuffsChanged(SyncList<BuffSystem.Buff>.Operation op, int index, BuffSystem.Buff oldBuff, BuffSystem.Buff newBuff)
    {
        switch (op)
        {
            case SyncList<BuffSystem.Buff>.Operation.OP_ADD:
                // index is where it was added into the list
                // newItem is the new item
                SetUpBuffsDisplays();
                break;
            case SyncList<BuffSystem.Buff>.Operation.OP_INSERT:
                // index is where it was inserted into the list
                // newItem is the new item
                break;
            case SyncList<BuffSystem.Buff>.Operation.OP_REMOVEAT:
                // index is where it was removed from the list
                // oldItem is the item that was removed
                SetUpBuffsDisplays();
                break;
            case SyncList<BuffSystem.Buff>.Operation.OP_SET:
                // index is of the item that was changed
                // oldItem is the previous value for the item at the index
                // newItem is the new value for the item at the index
                
                break;
            case SyncList<BuffSystem.Buff>.Operation.OP_CLEAR:
                // list got cleared
                break;
        }
    }

    public void SetUpBuffsDisplays()
    {
        if(gameObject.active == false)
        {
            return;
        }
        // Debug.Log(name + "UintFrame.OnbuffChanged called. Now I would update buffs");
        BuffHandler _bh = actor.GetComponent<BuffHandler>();
        for(int i = 0; i < buffDisplays.Length; i++)
        {
            if(i < _bh.Buffs.Count)
            {
                buffDisplays[i].gameObject.active = true;
                buffDisplays[i].AddBuff(_bh.Buffs[i]);
            }
            else
            {
                if(buffDisplays[i] != null){
                    buffDisplays[i].gameObject.active = false;
                }
            }
        }
        // UIManager.Instance.UpdateFrameBuffs(this);
    }
    

}
