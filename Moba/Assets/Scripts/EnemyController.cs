using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;

public class EnemyController : MonoBehaviour
{
    enum Command
    {

    }

    enum State
    {
        MoveToNextPoint,
        ChaseTarget,
        AttackTarget,
        Dying
    }

    public float arriveDistance;
    public float attackRange;
    public float attackTime = 1.0f;
    public float attackRate = 1.0f;
    float nextAttackTime = 0.0f;

    private Health health;
    private NavMeshAgent agent;
    private WayPointPath path;
    private int nextPathIndex = 0;
    //private bool isDying;
    [SyncVar] State currentState = State.MoveToNextPoint;
    private TargetingSystem targeting;
    Vector3 wayPointOffset;

    Weapon weapon;

    public void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        health = GetComponent<Health>();
        targeting = GetComponent<TargetingSystem>();
        weapon = GetComponent<Weapon>();
    }

    void Update()
    {
        switch (currentState)
        {
            case State.MoveToNextPoint:
                UpdateMoveTo();
                break;
            case State.ChaseTarget:
                UpdateChase();
                break;
            case State.AttackTarget:
                UpdateAttack();
                break;
            case State.Dying:
                UpdateDie();
                break;
        }
    }

    void UpdateMoveTo()
    {
        if (CheckDying() || IsTargetFound())
            return;

        Vector3 waypoint = path.GetWaypoint(nextPathIndex) + wayPointOffset;
        if (Vector2.Distance(waypoint.XZ(), transform.position.XZ()) <= arriveDistance)
        {
            nextPathIndex = path.GetNextIndex(nextPathIndex);
        }
        agent.SetDestination(path.GetWaypoint(nextPathIndex) + wayPointOffset);
    }

    [Command] void Cmd_Move(Vector3 destination)
    {
        Rpc_Move(destination);
    }
    [ClientRpc] void Rpc_Move(Vector3 destination)
    {
        mCurrentState = State.Move;
        agent.SetDestination(destination);
    }


    void UpdateChase()
    {
        if (CheckDying() || NoTarget() || TargetInRange())
            return;

        agent.SetDestination(targeting.GetCurrentTarget().transform.position);
    }

    void UpdateAttack()
    {
        if (CheckDying() || NoTarget() || TargetOutOfRange())
            return;

        if(nextAttackTime < Time.time)
        {
            weapon.Use(targeting.GetCurrentTarget());
            nextAttackTime = Time.time +  (1.0f / attackTime);
        }
    }

    void UpdateDie()
    {
        Vector3 position = transform.position;
        position.y = Mathf.Lerp(transform.position.y, transform.position.y - -5.0f, Time.deltaTime);
        transform.position = position;

        if (transform.position.y >= 50.0f)
            Destroy(this);
    }

    bool CheckDying()
    {
        if (!health.IsAlive())
        {
            GetComponent<Collider>().enabled = false;
            agent.enabled = false;
            currentState = State.Dying;
            return true;
        }
        return false;
    }

    bool IsTargetFound()
    {
        targeting.UpdateTarget();
        if (targeting.GetCurrentTarget())
            currentState = State.ChaseTarget;

        return targeting.GetCurrentTarget();
    }

    bool NoTarget()
    {
        targeting.UpdateTarget();
        if (!targeting.GetCurrentTarget())
            currentState = State.MoveToNextPoint;

        return targeting.GetCurrentTarget();
    }

    bool TargetInRange()
    {
        var target = targeting.GetCurrentTarget();
        if (Vector3.Distance(target.transform.position, transform.position) < attackRange)
        {
            currentState = State.AttackTarget;
            return true;
        }
        return false;
    }
    bool TargetOutOfRange()
    {
        var target = targeting.GetCurrentTarget();
        if (Vector3.Distance(target.transform.position, transform.position) > attackRange)
        {
            currentState = State.ChaseTarget;
            return true;
        }
        return false;
    }

    public void SetPath(WayPointPath waypointPath)
    {
        path = waypointPath;
        nextPathIndex = 0;

        wayPointOffset = transform.position - path.GetWaypoint(nextPathIndex);
        wayPointOffset.y = 0.0f;
    }


    void ChangeState(State s)
    {

    }

    [Command] void Cmd_ChangeState(State s)
    {

    }

    [ClientRpc] void Rpc_ChangeState(State s)
    {

    }
}
