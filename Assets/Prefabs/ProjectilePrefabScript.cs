using UnityEngine;

public class ProjectilePrefabScript : MonoBehaviour
{
    public float speed = 10.0f;

    //--------------------------------------
    //  Player Components 
    //--------------------------------------
    public Rigidbody2D body;
    public Collider2D collider;
    public Transform transform;

    // Awake is upon object construction
    private void Awake()
    {
        speed = 10.0f;
        transform = GetComponent<Transform>();
        body = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
    }

    // Start is called before the first frame update
    private void Start()
    {
        body.velocity = transform.right * speed;
    }

    // Update is called once per frame
    private void Update()
    {
        // body.velocity = transform.right * speed;
        // body.AddForce(
        //     new Vector2(
        //         speed * Time.deltaTime,
        //         body.velocity.y
        //     )
        // );
    }

    // Fixed Update is called once per frame
    private void FixedUpdate()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log($"CollisionEvent with {collision.gameObject.tag:s}");
    }

    /* private void OnTriggerEnter2D(Collider2D hitEntity) */
    /* { */
    /*     Debug.Log($"Projectile hit a {hitEntity.name:s}"); */
    /*     Destroy(gameObject); */
    /* } */

}
