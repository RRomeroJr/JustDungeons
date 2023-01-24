using UnityEngine;

public class EnemyWeaponScript : MonoBehaviour
{
    public GameObject projectilePrefab;
    private void Start()
    {
        //  Shooting pojectile every 3s
        InvokeRepeating("Shoot", 0.0f, 3.0f);
    }
    private void Shoot()
    {
        //  Richie: LATER make this spawn appropriate instead at the enemy's orgin
        GameObject projectile = Instantiate(
            projectilePrefab,
            gameObject.transform.position,
            gameObject.transform.rotation
        );

        Destroy(projectile, 2.0f);
        
    }
}
