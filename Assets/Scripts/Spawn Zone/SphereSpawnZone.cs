using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereSpawnZone : SpawnZone
{
    [SerializeField]
    private bool surfaceOnly;
    [SerializeField]
    private bool bidimensionalOnly;
    public override Vector3 SpawnPoint
    {
        get
        {
            Vector3 point;

            point = surfaceOnly ? Random.onUnitSphere : Random.insideUnitSphere;

            if (bidimensionalOnly)
            {
                point.y = 0;
            }

            return transform.TransformPoint(point);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireSphere(Vector3.zero, 1f);
    }
}
