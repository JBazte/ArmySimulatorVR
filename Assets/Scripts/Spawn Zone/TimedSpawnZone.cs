using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedSpawnZone : MonoBehaviour
{
    [SerializeField]
    private float spawnRate = 1f;

    [SerializeField]
    private SpawnZone zone;

    private float lastSpawn;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            zone.SpawnEnemy();
        }
        lastSpawn += Time.deltaTime * spawnRate;

        while (lastSpawn >= 1)
        {
            lastSpawn -= 1f;
            zone.SpawnEnemy();
        }
    }
    private void Start()
    {
        if (zone == null)
        {
            zone = GetComponentInChildren<SpawnZone>();
        }
    }

    private float ExpotentionalFunction(float x, float a, float b)
    {
        return a * x * x + b;
    }

}
