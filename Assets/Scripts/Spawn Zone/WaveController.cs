using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveController : PersistableObject
{
    [SerializeField]
    private float minimunWaveLenght = 10f;
    [SerializeField]
    private float waveMultiplier = 1f;

    [SerializeField]
    private int minimunEnemyPerWave = 1;
    [SerializeField]
    private float enemiesMultipier = 15f;

    [SerializeField]
    private TimedSpawnZone enemiesSpawnZone;

    private int enemyQuantity;
    private int currentWave = 0;
    private float waveTimeLeft;
    [SerializeField]
    private int changeFunction = 8;

    public System.Action OnWaveStart;

    void Start()
    {
        if (waveTimeLeft <= 0)
            StartNextWave();
    }

    private void Update()
    {
        waveTimeLeft -= Time.deltaTime;

        if (waveTimeLeft <= 0)
        {
            StartNextWave();
        }
    }


    private void StartNextWave()
    {
        currentWave++;
        waveTimeLeft = LogarithmFunction(currentWave, waveMultiplier, minimunWaveLenght);
        if (OnWaveStart != null)
        {
            OnWaveStart.Invoke();
        }
        enemyQuantity = Mathf.RoundToInt(ExpotentionalFunction(currentWave, 1 / enemiesMultipier, minimunEnemyPerWave));
        if (enemyQuantity > changeFunction)
        {
            enemyQuantity = Mathf.RoundToInt(LogarithmFunction(currentWave, enemiesMultipier, minimunEnemyPerWave));
        }
        enemiesSpawnZone.SetUnitSpawn(enemyQuantity);
    }

    public float LogarithmFunction(float x, float a, float b)
    {
        return (a * Mathf.Log10(x)) + b;
    }

    private float ExpotentionalFunction(float x, float a, float b)
    {
        return a * (x * x) + b;
    }

    public override void Save(GameDataWriter writer)
    {
        writer.Write(currentWave);
        writer.Write(waveTimeLeft);
    }
    public override void Load(GameDataReader reader)
    {
        currentWave = reader.ReadInt();
        waveTimeLeft = reader.ReadFloat();
    }
}
