using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempSpriteManager : MonoBehaviour
{
    public Sprite up;
    public Sprite down;
    public Sprite right;
    public Sprite left;

    public SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
     if ((Input.GetAxis("Horizontal") != 0) && (Input.GetAxis("Vertical") == 0)) // if a horizontal is held and no vertical
        {
            if(Input.GetAxis("Horizontal") > 0){
                //set image to right
                spriteRenderer.sprite = right;
            }
            else{
                //set image to left
                spriteRenderer.sprite = left;
            }
        }else if((Input.GetAxis("Vertical") != 0) && (Input.GetAxis("Horizontal") == 0)) // if a vertical is held and no horizontal
        {
            if(Input.GetAxis("Vertical") > 0){
                //set image to up
                spriteRenderer.sprite = up;
            }
            else{
                //set image to down
                spriteRenderer.sprite = down;
            }   
        }
    }
}
