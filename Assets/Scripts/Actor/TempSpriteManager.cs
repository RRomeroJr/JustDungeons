using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
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
    public Controller controller;

    void Start(){
        animator = GetComponent<Animator>();
        controller = GetComponent<Controller>();
    }
    
    

    void Update()
    {
        UpdateFacingDown();
        
        if(ShouldFlip()){
            flip();
        }
        
    }
    void flip(){
        gameObject.transform.localScale = Vector3.Scale(gameObject.transform.localScale, new Vector3(-1, 1, 1));
    }
    bool ShouldFlip(){
        
        if(controller.facingDirection.x >= 0.0f){
            if((transform.localScale.x >= 0.0f) == false){
                return true;
            }
        }
        else{
            if((transform.localScale.x < 0.0f) == false){
                return true;
            }
        }
        return false;

    }
    
    void UpdateFacingDown(){
        
        if(controller.facingDirection.y >= 0.0f){
            if(facingDown){
                facingDown = !facingDown;
                animator.SetBool("facingDown", facingDown);
            }
        }
        else{
            if(!facingDown){
                facingDown = !facingDown;
                animator.SetBool("facingDown", facingDown);
            }
        }
    }
   

}
