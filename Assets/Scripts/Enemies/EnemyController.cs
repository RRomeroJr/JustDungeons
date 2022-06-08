using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Enemy Settings")]
    public EnemySO enemyStats;

    [Header("Debug Settings")]
    public float currentHP;

    [Header("Optional Components")]
    [SerializeField] internal EnemyMovementScript movementScript;
    [SerializeField] internal EnemyWeaponScript weaponScript;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
