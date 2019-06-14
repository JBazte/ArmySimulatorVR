using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CharacterStats : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField]
    private float health;
    [SerializeField]
    private float damage;
    [SerializeField]
    private float speed;
    [SerializeField]
    private float attackSpeed;

    public float Speed
    {
        get
        {
            return speed;
        }
    }
    public float CurrentHealth
    {
        get
        {
            return currentHealth;
        }
    }
    public float AttackSpeed
    {
        get
        {
            return attackSpeed;
        }
    }
    public float Damage
    {
        get
        {
            return damage;
        }
    }
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
