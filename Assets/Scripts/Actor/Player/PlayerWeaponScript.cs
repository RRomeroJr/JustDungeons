using UnityEngine;

public class PlayerWeaponScript : MonoBehaviour
{

    public PlayerController playerController;
    public GameObject projectilePrefab;

    // Start is called before the first frame update
    private void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (playerController.inputScript.DetectFiringInput())
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        GameObject projectile = Instantiate(
            projectilePrefab,
            playerController.firePoint.position,
            playerController.transform.rotation
        );

        Destroy(projectile, 2.0f);
        
    }
}
