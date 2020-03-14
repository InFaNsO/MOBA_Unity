using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ProjectileController : MonoBehaviour
{
    public float moveSpeed;
    public float dmgAmount;

    private Vector3 lastTargetPosition;
    private Health targetHealth;

    private Vector3 GetBoundCenter(Health h)
    {
        Vector3 center = new Vector3();
        var colliders = h.GetComponentsInChildren<Collider>().Where(x => !x.isTrigger);
        foreach(var coll in colliders)
        {
            center += coll.bounds.center;
        }
        return center / colliders.Count();
    }

    private void Update()
    {
        if (targetHealth.IsAlive())
        {
            var collider = targetHealth.GetComponentInChildren<Collider>();
            if (collider)
                lastTargetPosition = collider.bounds.center;
            else
                lastTargetPosition = targetHealth.transform.position;

            lastTargetPosition = targetHealth.transform.position;
        }


        Vector3 newPosition = Vector3.MoveTowards(transform.position, lastTargetPosition, moveSpeed * Time.deltaTime);
        transform.position = newPosition;

        if (transform.position == lastTargetPosition)
        {
            Destroy(gameObject);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInParent<Health>() == targetHealth)
        {
            targetHealth.TakeDmg(dmgAmount);
            Destroy(gameObject);
        }
    }

    public void SetTarget(Health target)
    {
        targetHealth = target;
    }
}
