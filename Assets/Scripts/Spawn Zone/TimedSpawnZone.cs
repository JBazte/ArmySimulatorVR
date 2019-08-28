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


    bool haswaves;

    private float unitSpawn;
    private void Update()
    {
        lastSpawn += Time.deltaTime * spawnRate;
        while (lastSpawn >= 1)
        {
            lastSpawn -= 1f;
            if (unitSpawn > 0 || !haswaves)
            {
                unitSpawn--;
                zone.SpawnEnemy();
            }
            else { break; }
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            zone.SpawnEnemy();
        }
    }

    public void SetUnitSpawn(float count)
    {
        unitSpawn += count;
        haswaves = true;
    }
    private void Start()
    {
        if (zone == null)
        {
            zone = GetComponentInChildren<SpawnZone>();
        }
    }



}
