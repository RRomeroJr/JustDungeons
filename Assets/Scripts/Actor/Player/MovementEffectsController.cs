using System;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Handles movement effects that do special things to the player
/// </summary>
public class MovementEffectsController : MonoBehaviour
{
    private GameObject indicatorRef;
    public GameObject indicatorPrefab;
    public Vector2 moveDirection;
    private StatusEffectState currentEffectState;
    private NavMeshAgent agent;
    private Dictionary<StatusEffectState, int> effectDict;

    // Start is called before the first frame update
    void Start()
    {
        currentEffectState = StatusEffectState.None;
        moveDirection = Vector2.right;
        var buffHandler = GetComponent<BuffHandler>();
        buffHandler.StatusEffectChanged += HandleEffectChanged;
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentEffectState)
        {
            case StatusEffectState.Dizzy:
                ApplyDizzy();
                break;
            default:
                break;
        }
    }

    private void HandleEffectChanged(object sender, StatusEffectChangedEventArgs e)
    {
        effectDict = e.ToDictionary();
        // If new effect does not disable movement and the current effect has not ended, return early
        if (!StateDisables(e.NewEffect) && effectDict[currentEffectState] > 0)
        {
            return;
        }
        // End old State
        switch (currentEffectState)
        {
            case StatusEffectState.Dizzy:
                EndDizzy();
                break;
            case StatusEffectState.Feared:
                EndFear();
                break;
            default:
                break;
        }
        currentEffectState = e.NewEffect;
        // Start new State
        switch (currentEffectState)
        {
            case StatusEffectState.Dizzy:
                StartDizzy();
                break;
            case StatusEffectState.Feared:
                StartFear();
                break;
            default:
                break;
        }
    }

    public bool StateDisables(StatusEffectState state)
    {
        return state is StatusEffectState.Dizzy or StatusEffectState.Stunned or StatusEffectState.Feared;
    }

    #region Dizzy

    public void ApplyDizzy()
    {
        Vector2 inputVect = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (Mathf.Abs(inputVect.magnitude) > 0.0f)
        {
            //parentBuff.actor.GetComponent<Controller>().MoveInDirection(inputVect.magnitude * moveDirection);

            GetComponent<Controller>().moveDirection = moveDirection;
            Debug.DrawLine(transform.position, (moveDirection * 2.5f) + (Vector2)transform.position, Color.green);
        }
        else
        {
            //moveDirection = Quaternion.Euler(0, 0, power) * moveDirection;
            moveDirection = Quaternion.Euler(0, 0, 1) * moveDirection;
            indicatorRef.transform.up = moveDirection;

            Debug.DrawLine(transform.position, (moveDirection * 5.0f) + (Vector2)transform.position, Color.red);
        }
    }
    public void StartDizzy()
    {
        indicatorRef = Instantiate(indicatorPrefab, transform);
        //indicatorRef.GetComponent<FolllowObject>().target = this.gameObject;
        indicatorRef.transform.Rotate(moveDirection);
    }

    public void EndDizzy()
    {
        GetComponent<Controller>().moveDirection = null;
        Destroy(indicatorRef);
        moveDirection = Vector2.right;
    }

    #endregion

    #region Fear

    public void StartFear()
    {
        if (agent == null)
        {
            return;
        }
        if (HBCTools.NT_AuthoritativeClient(GetComponent<NetworkTransform>()))
        {
            agent.speed = GetComponent<Controller>().moveSpeed;
            Vector3 randomPointOnCircle = UnityEngine.Random.insideUnitCircle.normalized * 10;
            agent.SetDestination(transform.position + randomPointOnCircle);
        }
    }

    public void EndFear()
    {
        if (agent == null)
        {
            return;
        }
        if (HBCTools.NT_AuthoritativeClient(GetComponent<NetworkTransform>()))
        {
            agent.ResetPath();
        }
    }

    #endregion
}
