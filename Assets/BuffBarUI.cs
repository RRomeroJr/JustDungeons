using System.Collections.Generic;
using System.Linq;
using BuffSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuffUIContainer
{
    public GameObject prefab;
    public Image image;
    public Slider slider;
    public TextMeshProUGUI name;
    public TextMeshProUGUI remainingTime;

    public BuffUIContainer(GameObject go)
    {
        prefab = go;
        image = go.transform.GetChild(0).GetComponent<Image>();
        slider = go.transform.GetChild(1).GetComponent<Slider>();
        name = slider.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        remainingTime = slider.transform.GetChild(3).GetComponent<TextMeshProUGUI>();
    }
}

public class BuffBarUI : MonoBehaviour
{
    public GameObject buffItemPrefab;
    private IReadOnlyList<Buff> buffList;
    private List<BuffUIContainer> buffItemContainers;
    const int containerSize = 10;
    // Start is called before the first frame update
    void Start()
    {
        buffList = UIManager.playerActor.GetComponent<BuffHandler>().Buffs;
        buffItemContainers = new();
        for (int i = 0; i < containerSize; i++)
        {
            var b = new BuffUIContainer(Instantiate(buffItemPrefab, transform));
            buffItemContainers.Add(b);
        }
    }

    // Update is called once per fram
    void Update()
    {
        for (int i = 0; i < buffList.Count; i++)
        {
            buffItemContainers[i].prefab.SetActive(true);
        }
        for (int i = buffList.Count; i < containerSize; i++)
        {
            buffItemContainers[i].prefab.SetActive(false);
        }
        int j = 0;
        foreach (var buff in buffList)
        {
            buffItemContainers[j].slider.value = buff.remainingBuffTime / buff.buffSO.Duration;
            buffItemContainers[j].name.text = buff.buffSO.name;
            buffItemContainers[j].remainingTime.text = buff.remainingBuffTime.ToString("0.0");
            buffItemContainers[j].image.sprite = buff.buffSO.Icon;
            j++;
        }
    }
}
