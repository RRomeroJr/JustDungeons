using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryOrienter : MonoBehaviour
{
    public AbilityDelivery delivery;
    public bool facingRight = true;
    void Start()
    {
        delivery = GetComponent<AbilityDelivery>();
        Vector2 temp = delivery.target.transform.position - transform.position;
        transform.right = temp;
        if(delivery.target != null){
            if((Vector2.Distance((Vector2)delivery.target.transform.position, transform.position) < 0)
                &&
                (facingRight)){
                    flip();
            }
            if((Vector2.Distance((Vector2)delivery.target.transform.position, transform.position) > 0)
                &&
                (!facingRight)){
                    flip();
            }
        }
    }

    void Update()
    {
        
    }
    void flip(){
        Vector3 current = transform.localScale;
        current.x *= -1;
        transform.localScale = current;

        facingRight = !facingRight;
    }
}
