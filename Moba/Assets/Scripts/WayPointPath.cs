using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WayPointPath : MonoBehaviour
{
 private List<Transform> waypoints = new List<Transform>();

    void Start()
    {
        waypoints = new List<Transform>( GetComponentsInChildren<Transform>());
    }

    private void OnDrawGizmos()
    {
        for (int i = 0; i + 1 < waypoints.Count; ++i)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(waypoints[i].position, waypoints[i + 1].position);
        }
    }

    public Vector3 GetWaypoint(int index)
    {
        return waypoints[index].position;
    }

    public int GetNextIndex(int index)
    {
        return Mathf.Clamp(++index, 0, waypoints.Count - 1);
    }
}
