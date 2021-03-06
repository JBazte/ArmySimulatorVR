﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpawnZone : PersistableObject
{
    [SerializeField]
    public EnemyFactory[] factories;
    [SerializeField]
    Transform movePosition;

    public abstract Vector3 SpawnPoint { get; }

    public virtual Enemy SpawnEnemy()
    {

        int factoryIndex = Random.Range(0, factories.Length);
        Enemy enemy = factories[factoryIndex].GetWeighted();
        enemy.transform.position = SpawnPoint;
        if (movePosition != null)
        {
            if (enemy is EnemyController)
            {
                ((EnemyController)enemy).SetPoint(movePosition.position);
            }
            else
            {
                EnemyController[] childEnemys = enemy.GetComponentsInChildren<EnemyController>();
                if (childEnemys != null)
                {
                    foreach (var e in childEnemys)
                    {
                        e.SetPoint(movePosition.position);
                    }
                }
            }
        }
        return enemy;
    }

}
