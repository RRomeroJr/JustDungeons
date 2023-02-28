using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public struct BuffViewerContainer
{
    public string name;
    public float remainingTime;
    public int stacks;
}

public class BuffViewer : MonoBehaviour
{
    private BuffHandler buffHandler;
    [SerializeField] public List<BuffViewerContainer> buffList;

    // Start is called before the first frame update
    void Start()
    {
        buffHandler = GetComponent<BuffHandler>();
    }
    
    // Update is called once per frame
    void Update()
    {
        var buffData = buffHandler.Buffs.Select(x => new BuffViewerContainer
        {
            name = x.buffSO.name,
            remainingTime = x.remainingBuffTime,
            stacks = x.Stacks
        });
        buffList = new List<BuffViewerContainer>(buffData);
    }
}
