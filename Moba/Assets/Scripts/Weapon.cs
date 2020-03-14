using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] GameObject ProjectileObj;
    [SerializeField] Transform ProjectileSpawnPoint;

    [SerializeField] float damageAmount = 10.0f;

    public void Use(Health h)
    {
        if(ProjectileObj)
        {
            var projectile = Instantiate(ProjectileObj, ProjectileSpawnPoint.position, Quaternion.identity);
            var cont = projectile.GetComponent<ProjectileController>();
            cont.SetTarget(h);
            cont.dmgAmount = damageAmount;
        }
        else
            h.TakeDmg(damageAmount);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
