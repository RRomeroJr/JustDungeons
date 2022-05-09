using UnityEngine;

public class Enemy : MonoBehaviour
{
    public AudioSource source;
    public AudioClip clip;
    public Transform target;
    public float speed;
    public bool movingLeft;
    public SpriteRenderer sprite;

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    private void Start()
    {
        speed = 1;
    }

    // Update is called once per frame
    private void Update()
    {
        var step = speed * Time.deltaTime;

        //  Add additional logic to flip the sprite
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, step);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log($"CollisionEvent with {collision.gameObject.tag:s}");

        if (collision.gameObject.CompareTag("Player"))
        {
            source.PlayOneShot(clip);
            sprite.enabled = false;
            Destroy(gameObject, 1.2f);
        }
        else if (collision.gameObject.CompareTag("Projectile"))
        {
            Destroy(collision.gameObject);  //  Destroy the Projectile
            source.PlayOneShot(clip);
            sprite.enabled = false;
            Destroy(gameObject, 1.2f);      //  Destroy this gameObject
        }
    }
}
