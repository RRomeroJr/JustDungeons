using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ArenaSpawnerAtPosition : ArenaSpawner
{
	public Vector2 spawnPos;

    protected override Arena CreateArenaInstance()
    {
        Arena arenaRef = Instantiate(ArenaPrefab, spawnPos, Quaternion.identity);
        return arenaRef;
    }
    void OnDrawGizmosSelected()
    {
        if(TryGetComponent(out Actor _comp))
        {
            if(!_comp.inCombat)
            {
                // Display draw line from center to arenaspawn position
                Debug.DrawLine(transform.position, spawnPos, Color.red);
            }
        }
    }
}