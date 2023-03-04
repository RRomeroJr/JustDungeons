using System.Collections.Generic;
using System.Linq;
using BuffSystem;
using Mirror;
using UnityEngine;
using UnityEngine.Pool;

public class BuffBarUI : MonoBehaviour
{
    [SerializeField] private BuffBarItemUI buffItemPrefab;
    [SerializeField] private int containerSize = 10;
    private readonly List<BuffBarItemUI> activeBuffItems = new();
    private ObjectPool<BuffBarItemUI> pool;

    // Start is called before the first frame update
    void Start()
    {
        SyncList<Buff> buffList = UIManager.playerActor.GetComponent<BuffHandler>().Buffs;
        buffList.Callback += OnBuffListChanged;
        pool = new ObjectPool<BuffBarItemUI>(() =>
        {
            return Instantiate(buffItemPrefab, transform);
        }, buff =>
        {
            buff.gameObject.SetActive(true);
        }, buff =>
        {
            buff.gameObject.SetActive(false);
        }, buff =>
        {
            Destroy(buff.gameObject);
        }, false, 10, containerSize);

        // Display initial list
        foreach (var buff in buffList)
        {
            DisplayBuff(buff);
        }
    }

    private void OnBuffListChanged(SyncList<Buff>.Operation op, int itemIndex, Buff oldItem, Buff newItem)
    {
        if (op == SyncList<Buff>.Operation.OP_ADD)
        {
            DisplayBuff(newItem);
        }
        else if (op == SyncList<Buff>.Operation.OP_REMOVEAT)
        {
            RemoveBuff(oldItem);
        }
    }

    private void RemoveBuff(Buff oldItem)
    {
        BuffBarItemUI buff = activeBuffItems.FirstOrDefault(x => x.Buff == oldItem);
        activeBuffItems.Remove(buff);
        pool.Release(buff);
    }

    private void DisplayBuff(Buff newItem)
    {
        BuffBarItemUI buff = pool.Get();
        buff.AssignBuff(newItem);
        activeBuffItems.Add(buff);
    }
}
