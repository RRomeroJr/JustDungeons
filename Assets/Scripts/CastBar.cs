using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CastBar : MonoBehaviour
{
    public Text castName;
    public Image castFill;
    public Slider castBar;





    public IEnumerator castSpell(){
        
        yield return new WaitForSeconds(3.0f);
        Debug.Log("Cast Complete!");

    }

    void Update(){
        if(Input.GetKeyDown("r")){
            Debug.Log("Casting Spell..");
            StartCoroutine(castSpell());
        }
    }
}
