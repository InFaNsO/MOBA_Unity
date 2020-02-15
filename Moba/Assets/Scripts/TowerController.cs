using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerController : MonoBehaviour
{
    public GameObject projectile;
    public Transform turretTransform;
    public float fireRate;

    Health health;
    float nextAttackTime = 0.0f;

    TargetingSystem targeting;

    private void Start()
    {
        targeting = GetComponent<TargetingSystem>();
        health = GetComponent<Health>();
        nextAttackTime = 0.0f;
    }

    private void Update()
    {
        if (health.IsAlive())
        {
            targeting.UpdateTarget();
            Attack();
        }
    }

    private void Attack()
    {
        var targetHealth = targeting.GetCurrentTarget();
        if (targetHealth && nextAttackTime < Time.time)
        {
            // Fire Projectile at target
            var newProjectile = Instantiate(projectile, turretTransform);
            newProjectile.GetComponent<ProjectileController>().SetTarget(targetHealth);

            //Set nest time to fire
            nextAttackTime += 1.0f / fireRate;
        }
    }
}
