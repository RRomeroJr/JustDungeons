using Mirror;
using UnityEngine;

public class Pillar : NetworkBehaviour, IDamageable
{
    [SyncVar]
    [SerializeField] private int health;
    [Tooltip("Time till object is destroyed. If 0, object will not be destroyed over time.")]
    [SerializeField] private float despawnTimer;

    public int Health
    {
        get => health;
        set
        {
            if (value <= 0)
            {
                health = 0;
                Die();
                return;
            }
            health = value;
        }
    }

    private void Start()
    {
        Nameplate.Create(this);
        if (despawnTimer > 0)
        {
            Destroy(gameObject, despawnTimer);
        }
    }

    [Server]
    private void Die()
    {
        Destroy(gameObject);
    }

    [Server]
    public void Damage(float amount)
    {
        Health -= (int)amount;
    }
}
