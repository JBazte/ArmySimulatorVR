using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Granade : MonoBehaviour
{
    [SerializeField]
    private float explosionRadious = 3f;
    [SerializeField]
    LayerMask damageLayer;
    [SerializeField]
    float timeToExplote = 3f;
    [SerializeField]
    float damage = 5f;

    private void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadious, damageLayer);

        foreach (var col in colliders)
        {
            CharacterStats stats = col.GetComponentInParent<CharacterStats>();
            stats.TakeDamage(damage);
            Debug.Log(stats);
        }

        // Start Explosion Particles
        Destroy(gameObject);

    }

    private IEnumerator StartExplotion()
    {
        yield return new WaitForSeconds(timeToExplote);
        Explode();
    }

    private void Start()
    {
        StartCoroutine(StartExplotion());
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadious);

    }
}
