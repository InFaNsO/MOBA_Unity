using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    //is this visable?
    [SerializeField] float arrivalDistance = 1.0f;
    private NavMeshAgent agent;
    private WayPointPath path;
    int indexPath = 0;

    public void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public void SetPath(WayPointPath p)
    {
        path = p;
        indexPath = 0;
    }

    void Update()
    {
        if(Vector2.Distance(transform.position.XZ(), path[indexPath].XZ()) < arrivalDistance)
        {
            indexPath = path.GetNextIndex(indexPath);
        }
        agent.SetDestination(path[indexPath]);
    }
}
