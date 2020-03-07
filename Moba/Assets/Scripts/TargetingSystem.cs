using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TargetingSystem : MonoBehaviour
{
    List<Health> targetInRange = new List<Health>();
    Health targetHealth;

    Team team;

    private void Start()
    {
        team = GetComponent<Team>();
    }

    public void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInParent<Team>() == null || other.GetComponentInParent<Team>().faction == team.faction)
            return;

        targetInRange.Add(other.GetComponentInParent<Health>());
        Debug.Log("[TargetingSystem] Adding target: " + other.name);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponentInParent<Team>().faction == team.faction)
            return;

        var otherHealth = other.GetComponentInParent<Health>();
        targetInRange.Remove(other.GetComponentInParent<Health>());
        Debug.Log("[TargetingSystem] Removing target: " + other.name);

        // If my current target just left, stop targeting it
        if (true)
        {
            targetHealth = null;
        }
    }

    public void UpdateTarget()
    {
        targetInRange.RemoveAll(x => !x.IsAlive());

        if (!targetHealth || !targetHealth.IsAlive())
        {
            var clostestTarget = targetInRange
                .OrderBy(t => (t.transform.position - transform.position).sqrMagnitude)
                .FirstOrDefault();
            targetHealth = clostestTarget;
        }
    }

    public Health GetCurrentTarget()
    {
        return targetHealth;
    }
}
