using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
    Richie:
        Quick and dirty way of having the player change sprites
        based on movement. In the future probably should do this
        in a better way
*/
public class TempSpriteManager : MonoBehaviour
{
    public Sprite up;
    public Sprite down;
    // public Sprite right;
    // public Sprite left;
    public bool facingRight = true;
    public bool facingDown = true;
    public SpriteRenderer spriteRenderer;
    public Animator animator;

    void Start(){
        animator = GetComponent<Animator>();
    }
    void Update()
    {
    //  if ((Input.GetAxis("Horizontal") != 0) && (Input.GetAxis("Vertical") == 0)) // if a horizontal is held and no vertical
    //     {
    //         if(Input.GetAxis("Horizontal") > 0){
    //             //set image to right
    //             spriteRenderer.sprite = right;
    //         }
    //         else{
    //             //set image to left
    //             gameObject.transform.localScale = Vector3.Scale(gameObject.transform.localScale, new Vector3(-1, 1, 1));
    //         }
    //     }else if((Input.GetAxis("Vertical") != 0) && (Input.GetAxis("Horizontal") == 0)) // if a vertical is held and no horizontal
    //     {
    //         if(Input.GetAxis("Vertical") > 0){
    //             //set image to up
    //             spriteRenderer.sprite = up;
    //         }
    //         else{
    //             //set image to down
    //             spriteRenderer.sprite = down;
    //         }   
    //     }
        if((Input.GetAxis("Vertical") > 0)&&(facingDown == true)){
                //set image to up
                facingDown = !facingDown;
                //Debug.Log("Seting sprite to up");
                animator.SetBool("facingDown", facingDown);
                
        }
        else if((Input.GetAxis("Vertical") < 0)&&(facingDown == false)){
            facingDown = !facingDown;
            //Debug.Log("Seting sprite to down");
            //spriteRenderer.sprite = down;
            animator.SetBool("facingDown", facingDown);
            
        }
        if((Input.GetAxis("Horizontal") > 0)&&!(facingRight)){
                //set image to right
                flip();
                facingRight = !facingRight;
        }
        else if((Input.GetAxis("Horizontal") < 0)&&(facingRight)){
                //set image to right
                flip();
                facingRight = !facingRight;
        }
    }
    void flip(){
        gameObject.transform.localScale = Vector3.Scale(gameObject.transform.localScale, new Vector3(-1, 1, 1));
    }
}
