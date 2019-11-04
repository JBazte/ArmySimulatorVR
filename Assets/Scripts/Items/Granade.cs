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
    ParticleSystem explosion;
    [SerializeField]
    [Range(0, 3)]
    float explosionToScaleRadious = 0.5f;

    public bool hasTorus = true;
    private bool pressingTorus;

    private void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadious, damageLayer);

        foreach (var col in colliders)
        {
            CharacterStats stats = col.GetComponentInParent<CharacterStats>();
            float distance = Vector3.Distance(transform.position, stats.transform.position);
            float finalDamage = Mathf.Lerp(0, damage, 1 - (distance / explosionRadious));
            stats.TakeDamage(finalDamage / 2);
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
