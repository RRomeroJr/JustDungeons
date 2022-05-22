using UnityEngine;
using Pathfinding;

public class Enemy : MonoBehaviour
{
    public AudioSource source;
    public AudioClip clip;
    public Vector3 target;
    public Transform parent;
    public bool movingLeft;
    public SpriteRenderer sprite;

    public float currentHP;
    public EnemySO enemySO;
    public AIPath aipath;

    private void Start()
    {
        currentHP = enemySO.maxHP;
        sprite = GetComponent<SpriteRenderer>();
        parent = transform.parent;
        aipath = GetComponentInParent<AIPath>();
    }

    private void Update()
    {
        target = aipath.steeringTarget;
        var step = enemySO.moveSpeed * Time.deltaTime;
        if (!FacingTarget())
        {   //  Flipping enemy to face target if needed
            gameObject.transform.Rotate(0f, 180f, 0f);
        }
        parent.transform.position = Vector3.MoveTowards(parent.transform.position, target, step);
    }

    //  Returns true if enemy is facing it's target. If at same X postition will face right
    private bool FacingTarget()
    {
        if ((target.x >= parent.transform.position.x) && (parent.transform.rotation.y >= 0))
        {
            return true;
        }
        if ((target.x < parent.transform.position.x) && (parent.transform.rotation.y < 0))
        {
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
            Destroy(parent.gameObject, 1.2f);
        }
        else if (collision.gameObject.CompareTag("Projectile"))
        {
            Destroy(collision.gameObject);  //  Destroy the Projectile
            source.PlayOneShot(clip);
            sprite.enabled = false;
            Destroy(parent.gameObject, 1.2f);      //  Destroy this gameObject
        }
    }
}
