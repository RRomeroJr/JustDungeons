using UnityEngine;

public class Player : MonoBehaviour
{

    private Rigidbody2D body;

    // Start is called before the first frame update
    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        body.velocity = new Vector2(Input.GetAxis("Horizontal"), body.velocity.y); ;
    }
}
