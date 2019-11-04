using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shot : MonoBehaviour
{

    [SerializeField]
    float lifeSpam;
    protected LayerMask collideMask;
    protected float damage;

    private void OnCollisionEnter(Collision other)
    {

        OnImpact(other);
    }


    protected virtual void OnImpact(Collision other)
    {
        if (collideMask == (collideMask | (1 << other.gameObject.layer)))
        {
            float porsentage = 100f;
            var d = other.gameObject.GetComponent<HitBonuses>();
            if (d != null)
            {
                porsentage = d.damagePorsentage;
            }
            CharacterStats stats = other.transform.GetComponentInParent<CharacterStats>();

            if (stats != null)
            {
                stats.TakeDamage(damage * (porsentage / 100));
            }

            Destroy(gameObject);
        }
    }

    private void Start()
    {
        Destroy(gameObject, lifeSpam);
    }

    public void SetShot(float damage, LayerMask mask)
    {
        this.damage = damage;
        this.collideMask = mask;
    }
}
