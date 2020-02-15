using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float maxHealth;

    private float currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }
    public void TakeDmg(float amount)
    {
        if (currentHealth > 0.0f)
        {
            currentHealth -= amount;
            currentHealth = Mathf.Max(currentHealth, 0.0f);
            Debug.Log("[health] Lost " + amount + "hp. Current Health: " + currentHealth);
        }
    }

    public bool IsAlive()
    {
        return currentHealth > 0.0f;
    }
}
