using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeightSpawnZone : MonoBehaviour
{
    [SerializeField]
    SpawnZone spawnZone;

    [SerializeField]
    int[] maxCount;
    [SerializeField]
    float[] importance;
    private void ChangeWeights()
    {
        foreach (var f in spawnZone.factories)
        {
            float[] modWeights = new float[maxCount.Length];
            for (int i = 0; i < maxCount.Length; i++)
            {
                int count = f.ActiveInstances(i);
                int difference = maxCount[i] - count;
                modWeights[i] = difference * importance[i];


            }
            f.SpawnWeights = modWeights;
        }

    }

    private void Start()
    {
        foreach (var f in spawnZone.factories)
        {
            f.OnInstanceCountChange += ChangeWeights;
        }
    }
}
