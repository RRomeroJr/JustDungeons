using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EnemySO : ScriptableObject
{
    public new string name;
    public Sprite sprite;

    public FloatReference maxHP;
    public FloatReference moveSpeed;
    public FloatReference patrolSpeed;
}
