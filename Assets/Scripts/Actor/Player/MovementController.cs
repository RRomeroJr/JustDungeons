using Mirror;
using UnityEngine;
using UnityEngine.AI;

public class MovementEffectsController : MonoBehaviour
{
    private GameObject indicatorRef;
    public GameObject indicatorPrefab;
    public Vector2 moveDirection;
    private ActorState actorState;
    private NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        actorState = ActorState.Free;
        moveDirection = Vector2.right;
        var actor = GetComponent<Actor>();
        actor.StateChanged += HandleStateChanged;
    }

    // Update is called once per frame
    void Update()
    {
        switch (actorState)
        {
            case ActorState.Dizzy:
                ApplyDizzy();
                break;
            default:
                break;
        }
    }

    private void HandleStateChanged(object sender, StateChangedEventArgs e)
    {
        if (e.ActorState == actorState)
        {
            return;
        }
        // End old State
        switch (actorState)
        {
            case ActorState.Dizzy:
                EndDizzy();
                break;
            case ActorState.Feared:
                EndFear();
                break;
            default:
                break;
        }
        actorState = e.ActorState;
        // Start new State
        switch (actorState)
        {
            case ActorState.Dizzy:
                StartDizzy();
                break;
            case ActorState.Feared:
                StartFear();
                break;
            default:
                break;
        }
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
        indicatorRef = Instantiate(indicatorPrefab, transform.position, Quaternion.identity);
        indicatorRef.GetComponent<FolllowObject>().target = this.gameObject;
        indicatorRef.transform.Rotate(moveDirection);
    }

    public void EndDizzy()
    {
        GetComponent<Controller>().moveDirection = null;
        Destroy(indicatorRef);
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
            agent.speed = GetComponent<Controller>().moveSpeed * GetComponent<ISpeedModifier>().SpeedModifier;
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
