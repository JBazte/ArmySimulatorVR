using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompositeSpawnPosition : SpawnZone
{
    [SerializeField]
    private SpawnZone[] spawnZones;
    [SerializeField]
    private bool sequencial = false;
    [SerializeField]
    private bool overrideConf = false;

    private int lastSpawn;
    public override Vector3 SpawnPoint
    {
        get
        {
            int index;
            if (!sequencial)
            {
                index = Random.Range(0, spawnZones.Length);
            }
            else
            {
                index = lastSpawn++;
                if (lastSpawn >= spawnZones.Length)
                {
                    lastSpawn = 0;
                }
            }
            return spawnZones[index].SpawnPoint;
        }
    }
    public override Enemy SpawnEnemy()
    {
        Enemy e;
        if (overrideConf)
        {
            e = base.SpawnEnemy();
        }
        else
        {
            int index;
            if (sequencial)
            {
                index = lastSpawn++;
                if (lastSpawn >= spawnZones.Length)
                {
                    lastSpawn = 0;
                }
            }
            else
            {
                index = Random.Range(0, spawnZones.Length);
            }
            e = spawnZones[index].SpawnEnemy();

        }
        return e;
    }
}
