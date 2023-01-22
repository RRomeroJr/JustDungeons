using UnityEngine;

public class PlayerWeaponScript : MonoBehaviour
{

    public PlayerControllerOLD PlayerControllerOLD;
    public GameObject projectilePrefab;

    // Start is called before the first frame update
    private void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerControllerOLD.inputScript.DetectFiringInput())
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        GameObject projectile = Instantiate(
            projectilePrefab,
            PlayerControllerOLD.firePoint.position,
            PlayerControllerOLD.transform.rotation
        );

        Destroy(projectile, 2.0f);
        
    }
}
