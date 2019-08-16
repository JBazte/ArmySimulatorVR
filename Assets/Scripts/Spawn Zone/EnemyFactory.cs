using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu]
public class EnemyFactory : ScriptableObject
{


    [SerializeField]
    Enemy[] prefabs;

    [SerializeField]
    private bool recycle;

    List<Enemy>[] pools;

    Scene poolScene;

    public Enemy Get(int enemyId = 0)
    {
        Enemy instance;

        if (recycle)
        {
            if (pools == null)
            {
                CreatePools();
            }
            List<Enemy> pool = pools[enemyId];
            int lastIndex = pool.Count - 1;
            if (lastIndex >= 0)
            {
                instance = pool[lastIndex];
                instance.gameObject.SetActive(true);
                pool.RemoveAt(lastIndex);
            }
            else
            {
                instance = Instantiate(prefabs[enemyId]);
                instance.OriginFactory = this;
                instance.EnemyID = enemyId;
                SceneManager.MoveGameObjectToScene(
                    instance.gameObject, poolScene
                );
            }
        }
        else
        {
            instance = Instantiate(prefabs[enemyId]);
            instance.EnemyID = enemyId;
        }
        return instance;

    }

    public void Reclaim(Enemy enemy)
    {
        if (enemy.OriginFactory != this)
        {
            Debug.LogError("Trying to Reclaim an Enemy from a different Factory");
            return;
        }

        if (recycle)
        {
            if (pools == null)
            {
                CreatePools();
            }

            pools[enemy.EnemyID].Add(enemy);
            enemy.gameObject.SetActive(false);

        }
        else
        {
            Destroy(enemy.gameObject);
        }

    }


    public Enemy GetRandom()
    {
        return Get(Random.Range(0, prefabs.Length));
    }

    void CreatePools()
    {
        pools = new List<Enemy>[prefabs.Length];

        for (int i = 0; i < pools.Length; i++)
        {
            pools[i] = new List<Enemy>();
        }
        poolScene = SceneManager.CreateScene(name);
    }

}


