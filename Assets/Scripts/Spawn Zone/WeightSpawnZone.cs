using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeightSpawnZone : MonoBehaviour
{
    [SerializeField]
    SpawnZone spawnZone;


    private void ChangeWeights()
    {
        foreach (var f in spawnZone.factories)
        {
            //  f.se
        }

    }
}
