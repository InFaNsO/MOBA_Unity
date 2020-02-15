using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public float moveSpeed;
    public float dmgAmount;

    private Vector3 lastTargetPosition;
    private Health targetHealth;

    private void Update()
    {
        if (targetHealth.IsAlive())
        {
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
