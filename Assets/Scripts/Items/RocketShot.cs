using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketShot : Shot
{
    [SerializeField]
    private float explosionRadious = 3f;

    protected override void OnImpact(Collision other)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadious, collideMask);

        foreach (var col in colliders)
        {
            CharacterStats stats = col.GetComponentInParent<CharacterStats>();
            stats.TakeDamage(damage);
            Debug.Log(stats);
        }

        // Start Explosion Particles
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadious);

    }
}
