using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu]
public class EnemyFactory : ScriptableObject
{


    [SerializeField]
    Enemy[] prefabs;
    [SerializeField]
    float[] spawnWeights;


    float[] modifingWeights;
    private int[] instancesAlive;
    public float[] SpawnWeights
    {
        set
        {
            modifingWeights = value;
        }

    }
    public bool isWeighted;
    private float totalWeight;
    [SerializeField]
    private bool recycle;
    public System.Action OnInstanceCountChange;
    List<Enemy>[] pools;

    Scene poolScene;

    public int FactoryID
    {
        get
        {
            return factoryID;
        }
        set
        {
            if (value != int.MinValue && factoryID == int.MinValue)
            {
                factoryID = value;
            }
        }
    }
    [System.NonSerialized]
    private int factoryID = int.MinValue;
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
            instancesAlive[enemyId]++;
            if (OnInstanceCountChange != null)
            {
                OnInstanceCountChange.Invoke();
            }
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
        GameController.instance.AddEnemy(instance);

        return instance;

    }



    public void Reclaim(Enemy enemy)
    {
        if (enemy.OriginFactory != this)
        {
            Debug.LogError("Trying to Reclaim an Enemy from a different Factory");
            return;
        }
        instancesAlive[enemy.EnemyID]--;
        if (OnInstanceCountChange != null)
        {
            OnInstanceCountChange.Invoke();
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
            GameController.instance.RemoveEnemy(enemy);
            Destroy(enemy.gameObject);
        }

    }


    public Enemy GetRandom()
    {
        return Get(Random.Range(0, prefabs.Length));
    }

    public Enemy GetWeighted()
    {
        if (!isWeighted || prefabs.Length != spawnWeights.Length)
        {
            return GetRandom();

        }
        else
        {
            if (modifingWeights.Length == 0 || modifingWeights == null)
                modifingWeights = new float[prefabs.Length];

            spawnWeight[] weights = new spawnWeight[prefabs.Length];
            float currenttotalWeight = 0;
            for (int i = 0; i < prefabs.Length; i++)
            {
                weights[i].weight = spawnWeights[i];
                if (weights[i].weight < 0)
                {
                    weights[i].weight = 0;

                }
                weights[i].fromweight = currenttotalWeight;
                //float bothWeights = ;
                currenttotalWeight += weights[i].weight + modifingWeights[i];
                weights[i].toweight = currenttotalWeight;


            }
            totalWeight = currenttotalWeight;
            for (int i = 0; i < weights.Length; i++)
            {
                weights[i].probability = ((weights[i].weight + modifingWeights[i]) / totalWeight) * 100;
                Debug.Log(prefabs[i].name + " Probabilities are: " + weights[i].probability + "%");

            }
            float pickNumber = Random.Range(0, totalWeight);

            for (int i = 0; i < weights.Length; i++)
            {
                if (pickNumber > weights[i].fromweight && pickNumber < weights[i].toweight)
                {
                    return Get(i);
                }
            }
            return GetRandom();
        }
    }

    void CreatePools()
    {
        pools = new List<Enemy>[prefabs.Length];
        instancesAlive = new int[prefabs.Length];
        for (int i = 0; i < pools.Length; i++)
        {
            pools[i] = new List<Enemy>();
        }
        poolScene = SceneManager.CreateScene(name);
    }

    [System.Serializable]
    public struct spawnWeight
    {
        public float weight;
        public float fromweight;
        public float toweight;
        public float probability;


    }

    public int ActiveInstances(int i)
    {
        return instancesAlive[i];
    }

}


