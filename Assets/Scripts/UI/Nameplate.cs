using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
/*
    Container with references important to controlling
    and displaying unit frames properly
*/
public class Nameplate : MonoBehaviour
{
    const float unslectedScale = 0.01f;
    const float selectedScale = 0.0125f;
    public Text unitName;
    public Image healthFill;
    public Image resourceFill;
    public Slider healthBar;
    public Slider resourceBar;
    public CastBar castBar;
    public Actor actor;
    public IDamageable damageable;
    public Vector2 offset;
    public Canvas canvas;
    private Renderer actorRenderer;
    public UnityEvent<bool> selectedEvent = new UnityEvent<bool>();

    void Awake()
    {
        offset = new Vector2(0f, 1.5f);
    }

    void Start()
    {
        healthBar = transform.GetChild(1).GetComponent<Slider>();
        resourceBar = transform.GetChild(2).GetComponent<Slider>();
        canvas = GetComponentInParent<Canvas>();
        selectedEvent.AddListener(SetSelectedScale);

        if (actor != null)
        {
            unitName.text = actor.ActorName;
            castBar.caster = actor.abilityHandler;
            actor.abilityHandler.OnCastStarted.AddListener(OnCastStarted);
            actorRenderer = actor.GetComponent<Renderer>();
        }
        else
        {
            healthBar.maxValue = damageable.Health;
            healthBar.value = damageable.Health;
            unitName.text = transform.parent.parent.name;
            resourceBar.gameObject.SetActive(false);
            castBar.gameObject.SetActive(false);
            resourceFill.gameObject.SetActive(false);
        }
    }

    void OnCastStarted()
    {
        if(castBar.gameObject.active == false)
        {
            castBar.gameObject.active = true;
        }
        castBar.OnAbilityChanged();
    }

    public static Nameplate Create(Actor _actor)
    {
        Nameplate npRef = (Instantiate(UIManager.nameplatePrefab) as GameObject).GetComponentInChildren<Nameplate>();
        npRef.transform.position = _actor.transform.position + (Vector3)npRef.offset;
        npRef.actor = _actor;
        return npRef;
    }

    public static Nameplate Create(IDamageable d)
    {
        Nameplate npRef = Instantiate(UIManager.nameplatePrefab, (d as MonoBehaviour).transform).GetComponentInChildren<Nameplate>();
        npRef.transform.position += (Vector3)npRef.offset;
        npRef.damageable = d;
        return npRef;
    }

    void Update()
    {
        if ((actor == null || !actor.gameObject.active) && damageable == null)
        {
            Destroy(canvas.gameObject);
            return;
        }
        if (actor != null)
        {
            transform.position = actor.transform.position + (Vector3)offset;
            UpdateSliderHealth();
            UpdateSliderResource(resourceBar);
            canvas.sortingOrder = actorRenderer.sortingOrder;
        }
        else if (damageable != null)
        {
            healthBar.value = damageable.Health;
        }
    }

    void SetSelectedScale(bool _selected)
    {
        if (_selected)
        {
            canvas.gameObject.transform.localScale = new Vector3(selectedScale, selectedScale, 1);
        }
        else
        {
            canvas.gameObject.transform.localScale = new Vector3(unslectedScale, unslectedScale, 1);
        }
    }

    void UpdateSliderHealth()
    {
        healthBar.maxValue = actor.MaxHealth;
        healthBar.value = actor.Health;
    }

    void UpdateSliderResource(Slider silder)
    {
        if (actor.ResourceTypeCount() > 0)
        {
            silder.maxValue = actor.getResourceMax(0);
            silder.value = actor.getResourceAmount(0);
        }
    }
}
