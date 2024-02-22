using Mirror;
using TheKiwiCoder;
using UnityEngine;

public class SpawnPrefab : ActionNode
{
    public GameObject prefab;
    public Vector2 relativePosition;
    public string name;

    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        GameObject spawnedObject = Instantiate(prefab, context.transform.position + (Vector3)relativePosition, Quaternion.identity);
        spawnedObject.name = name;
        if (spawnedObject == null)
        {
            return State.Failure;
        }
        NetworkServer.Spawn(spawnedObject);
        return State.Success;
    }
}
