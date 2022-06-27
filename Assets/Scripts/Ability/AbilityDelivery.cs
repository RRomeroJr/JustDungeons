using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityDelivery : MonoBehaviour
{
    public AbilityEffect AAE;
    public Vector3 skillShotTarget;
    public Actor caster;
    public Actor target;
    public int type; // 0 detroys when reaches target, 1 = skill shot
    public float speed;
    void Start()
    {
        skillShotTarget = getSkillShotTarget();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {   
        if(type == 0){   
            if (other.gameObject.GetComponent<Actor>() == target){
                other.gameObject.GetComponent<Actor>().applyAbilityEffect(AAE, caster);
                Destroy(gameObject);
            }
        }
        if(type == 1){
            if (other.gameObject.GetComponent<Actor>() != caster){
                if (other.gameObject.GetComponent<Actor>() != null){
                    other.gameObject.GetComponent<Actor>().applyAbilityEffect(AAE, caster);
                }
                Destroy(gameObject);
            }
        }
    }
    void Update()
    {
        if(type == 0){
            transform.position = Vector2.MoveTowards(transform.position, target.gameObject.transform.position, speed);
        }
        if(type == 1){
            transform.position = Vector2.MoveTowards(transform.position, skillShotTarget, speed);
        }
    }

    /*public AbilityDelivery(ActiveAbilityEffect _AAE, int _type, Actor _caster){
        AAE = _AAE;
        type = _type;
        caster = _caster;
    }*/
    public void init(AbilityEffect _AAE, int _type, Actor _caster, Actor _target, float _speed){
        gameObject.transform.position = _caster.gameObject.transform.position;
        AAE = _AAE;
        type = _type;
        caster = _caster;
        target = _target;
        speed = _speed;
    }
    Vector3 getSkillShotTarget(){
        Vector3 scrnPos = Input.mousePosition;
        return Camera.main.ScreenToWorldPoint(scrnPos);

    }
}
