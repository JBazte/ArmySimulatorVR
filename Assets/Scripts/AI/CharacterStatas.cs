using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStatas : MonoBehaviour
{
    [SerializeField]
    private float health;
    [SerializeField]
    private float damage;
    [SerializeField]
    private float speed;
    [SerializeField]
    private float attackSpeed;

    private float currentHealth;

    private void Start()
    {
        currentHealth = health;
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }

}
