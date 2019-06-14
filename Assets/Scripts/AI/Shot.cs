using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shot : MonoBehaviour
{

    private LayerMask collideMask;
    private float damage;

    private void OnCollisionEnter(Collision other)
    {
        if (collideMask == (collideMask | (1 << other.gameObject.layer)))
        {
            CharacterStats stats = other.transform.GetComponentInParent<CharacterStats>();
            if (stats != null)
            {
                stats.TakeDamage(damage);
            }

            Destroy(gameObject);
        }
    }

    public void SetShot(float damage, LayerMask mask)
    {
        this.damage = damage;
        this.collideMask = mask;
    }
}
