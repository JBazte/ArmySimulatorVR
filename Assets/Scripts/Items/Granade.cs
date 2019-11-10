using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class Granade : ThrowableObject
{
    [SerializeField]
    private float explosionRadious = 3f;
    [SerializeField]
    LayerMask damageLayer;
    [SerializeField]
    float timeToExplote = 5f;
    [SerializeField]
    float damage = 125f;
    [SerializeField]
    float explosionForce = 1f;

    [SerializeField]
    ParticleSystem explosion;
    [SerializeField]
    [Range(0, 3)]
    float explosionToScaleRadious = 0.5f;

    public bool hasTorus = true;
    private bool pressingTorus;

    private void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadious);

        foreach (var col in colliders)
        {
            float distance = Vector3.Distance(transform.position, col.transform.position);
            if (((1 << col.gameObject.layer) & damageLayer) != 0)
            {
                //It matched one
                CharacterStats stats = col.GetComponentInParent<CharacterStats>();
                float finalDamage = Mathf.Lerp(0, damage, 1 - (distance / explosionRadious));
                stats.TakeDamage(finalDamage / 2);
            }
            Rigidbody rb = col.GetComponentInParent<Rigidbody>();
            if (rb != null)
            {
                float finalForceAmount = Mathf.Lerp(0, explosionForce, 1 - (distance / explosionRadious));
                Vector3 direction = (this.transform.position - rb.transform.position).normalized;
                rb.AddForce(-direction * finalForceAmount, ForceMode.Impulse);
            }

            //Debug.Log(finalDamage);
        }

        var e = Instantiate(explosion, transform.position, Quaternion.identity);
        e.transform.localScale = Vector3.one * explosionToScaleRadious * explosionRadious;
        Destroy(e, 5f);
        Destroy(gameObject);

    }

    private IEnumerator StartCooking()
    {
        yield return new WaitForSeconds(timeToExplote);
        Explode();
    }

    private void Update()
    {
        if (pressingTorus && !hasTorus)
        {
            StartCoroutine(StartCooking());
        }
    }
    protected override void OnAttachedToHand(Hand hand)
    {
        base.OnAttachedToHand(hand);
        pressingTorus = true;

    }
    protected override void OnDetachedFromHand(Hand hand)
    {
        base.OnDetachedFromHand(hand);
        pressingTorus = false;

    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadious);

    }
}
