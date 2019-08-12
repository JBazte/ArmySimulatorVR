using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpawnZone : MonoBehaviour
{
    [SerializeField]
    EnemyFactory factory;


    public abstract Vector3 position { get; }

    public virtual EnemyController SpawnEnemy()
    {

    }
}
