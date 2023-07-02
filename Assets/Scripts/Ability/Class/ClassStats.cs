using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName="ClassStats", menuName = "HBCsystem/Classes/ClassStats")]
public class ClassStats: ScriptableObject
{
	// public int health;
    public float healthMutliplier = 1.0f;
    public float mainStat = 100.0f;
    public float HealingPower = 10.0f;
    
}