using UnityEngine;

public class LerpScale : MonoBehaviour
{
    [SerializeField] private Vector3 startScale;
    [SerializeField] private Vector3 endScale;
    [SerializeField] private float duration;

    private float endTime;

    void Start()
    {
        endScale = transform.localScale;
        transform.localScale = startScale;
        endTime = Time.time + duration;
    }

    void Update()
    {
        float t = (endTime - Time.time) / duration;
        transform.localScale = Vector3.Lerp(endScale, startScale, t);
    }
}
