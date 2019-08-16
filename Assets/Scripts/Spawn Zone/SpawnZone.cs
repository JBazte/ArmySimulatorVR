using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpawnZone : PersistableObject
{
    [SerializeField]
    EnemyFactory[] factories;
    [SerializeField]
    Transform movePosition;

    public abstract Vector3 SpawnPoint { get; }

    public virtual Enemy SpawnEnemy()
    {
        int factoryIndex = Random.Range(0, factories.Length);
        Enemy enemy = factories[factoryIndex].GetRandom();
        enemy.transform.position = SpawnPoint;
        if (movePosition != null)
        {
            if (enemy is EnemyController)
            {
                ((EnemyController)enemy).SetPoint(movePosition);
            }
        }
        return enemy;
    }

}
