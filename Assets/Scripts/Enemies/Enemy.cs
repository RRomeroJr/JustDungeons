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
    private void Start()
    {
        speed = 1;
    }
    private void Update()
    {   
        
    }
    private void FixedUpdate(){
        var step = speed * Time.deltaTime;
        if(!FacingTarget()){   //  Flipping enemy to face target if needed
            gameObject.transform.Rotate(0f, 180f, 0f);
        }
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, step);
    }
    
    //  Returns true if enemy is facing it's target. If at same X postition will face right
    bool FacingTarget(){
        if((target.position.x >= gameObject.transform.position.x) && (gameObject.transform.rotation.y >= 0)){
            return true;
        }
        if((target.position.x < gameObject.transform.position.x) && (gameObject.transform.rotation.y < 0)){
            return true;
        }
        return false;
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
