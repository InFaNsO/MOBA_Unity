using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WayPointPath : MonoBehaviour
{
    private List<Transform> wayPoints = new List<Transform>();

    private void Start()
    {
        wayPoints = new List<Transform>( GetComponentsInChildren<Transform>());
        wayPoints.RemoveAt(0);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        if (wayPoints.Count == 0)
            return;

        Gizmos.DrawSphere(wayPoints[0].position, 0.50f);

        for (int i = 1; i < wayPoints.Count; ++i)
        {
            Gizmos.DrawSphere(wayPoints[i].position, 0.50f);
            Gizmos.DrawLine(wayPoints[i - 1].position, wayPoints[i].position);
        }
    }

    public int GetNextIndex(int index)
    {
        return Mathf.Clamp(++index, 0, wayPoints.Count - 1);
    }

    public Vector3 this[int i]
    {
        get { return wayPoints[i].position; }
    }
}
