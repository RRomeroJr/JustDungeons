using TMPro;
using UnityEngine;
using UnityEngine.UI;
using BuffSystem;

public class BuffBarItemUI : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI buffName;
    [SerializeField] private TextMeshProUGUI remainingTime;
    public Buff Buff { get; private set; }

    // Update is called once per frame
    void Update()
    {
        slider.value = Buff.RemainingBuffTime / Buff.BuffSO.Duration;
        remainingTime.text = Buff.RemainingBuffTime.ToString("0.0");
    }

    public void AssignBuff(Buff b)
    {
        Buff = b;
        image.sprite = Buff.BuffSO.Icon;
        buffName.text = Buff.BuffSO.name;
    }

    private void OnDisable()
    {
        Buff = null;
    }
}
