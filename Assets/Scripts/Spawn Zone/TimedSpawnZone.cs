using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedSpawnZone : PersistableObject
{
    [SerializeField]
    private float spawnRate = 1f;

    [SerializeField]
    private SpawnZone zone;

    private float lastSpawn = -1;


    bool haswaves;

    private int unitSpawn = int.MinValue;
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

    public void SetUnitSpawn(int count)
    {
        if (unitSpawn == int.MinValue)
        {
            unitSpawn = count;
        }
        else
        {
            unitSpawn += count;
        }
        haswaves = true;
    }
    private void Start()
    {
        if (zone == null)
        {
            zone = GetComponentInChildren<SpawnZone>();
        }
        if (lastSpawn < 0)
        {
            lastSpawn = 1f;
        }
    }

    public override void Save(GameDataWriter writer)
    {
        writer.Write(unitSpawn);
        writer.Write(lastSpawn);
    }

    public override void Load(GameDataReader reader)
    {
        unitSpawn = reader.ReadInt();
        lastSpawn = reader.ReadFloat();
        Debug.Log(unitSpawn);
        if (unitSpawn != int.MinValue)
        {
            haswaves = true;
        }
    }



}
