using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CharacterStats : PersistableObject
{
    [Header("Stats")]
    [SerializeField]
    private float health = 100;
    [SerializeField]
    private float damage = 10;
    [SerializeField]
    private float speed = 5;
    [SerializeField]
    private float attackSpeed = 3;
    [SerializeField]
    [Range(0, 100)]
    private float accuracy = 80;
    [SerializeField]
    private int maxAmmo = 30;
    [SerializeField]
    private float reloadTime = 3;


    bool isDisolving;


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

    public float MaxHealth
    {
        get
        {
            return health;
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
    public int MaxAmmo
    {
        get
        {
            return maxAmmo;
        }
    }
    public float Accuracy
    {
        get
        {
            return accuracy;
        }
    }
    public float ReloadTime
    {
        get
        {
            return reloadTime;
        }
    }
    private float currentHealth;

    private void Awake()
    {
        currentHealth = health;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            TakeDamage(10f);
        }
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            if (!isDisolving)
                StartCoroutine(StartDisolve());
        }
    }

    public bool HealDamage(float amount)
    {
        if (currentHealth + amount > health)
        {
            amount = health - currentHealth;
            currentHealth += amount;
            return true;
        }
        else
        {
            currentHealth += amount;
            return false;
        }



    }
    public void Reset()
    {
        currentHealth = MaxHealth;
    }
    private IEnumerator StartDisolve()
    {
        isDisolving = true;
        float time = GetComponentInChildren<SpawnEffect>().StartDisolve();
        if (time < 0) { time = 0; }
        GetComponentInChildren<Collider>().enabled = false;
        yield return new WaitForSeconds(time);
        GetComponentInChildren<Collider>().enabled = false;
        isDisolving = false;
        Die();
    }

    protected virtual void Die()
    {
        Enemy e = GetComponentInChildren<Enemy>();
        if (e != null)
        {
            Reset();
            e.Recycle();
        }
        else
        {
            Destroy(gameObject);
        }
    }

}
