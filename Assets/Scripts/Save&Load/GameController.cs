using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
[DisallowMultipleComponent]
public class GameController : PersistableObject
{
    public static GameController instance;
    [SerializeField]
    PersistantStorage storage;
    public PersistantStorage Storage
    {
        get
        {
            return storage;
        }

    }
    [SerializeField]
    ScoreController scoreController;
    [SerializeField]
    public UnitSelector unitSelector;
    const int saveVersion = 0;

    [SerializeField]
    private KeyCode saveKey;
    [SerializeField]
    private KeyCode loadKey;
    [SerializeField]
    EnemyFactory[] factories;
    private List<Enemy> enemies;
    private int buildIndex;
    private KeyCode[] keyCodes = {
         KeyCode.Alpha1,
         KeyCode.Alpha2,
         KeyCode.Alpha3,
         KeyCode.Alpha4,
         KeyCode.Alpha5,
         KeyCode.Alpha6,
         KeyCode.Alpha7,
         KeyCode.Alpha8,
         KeyCode.Alpha9,
     };
    public System.Action OnFinishLevel;
    private void Awake()
    {
        instance = this;
        enemies = new List<Enemy>();
        //storage = new PersistantStorage();
        buildIndex = SceneManager.GetActiveScene().buildIndex;
        if (storage == null)
        {
            storage = FindObjectOfType<PersistantStorage>();
        }
        if (scoreController == null)
        {
            scoreController = FindObjectOfType<ScoreController>();
        }
        if (unitSelector == null)
        {
            unitSelector = FindObjectOfType<UnitSelector>();
        }
    }

    private void Update()
    {

        if (Input.GetKeyDown(saveKey))
        {
            storage.Save(this, saveVersion);
        }
        if (Input.GetKeyDown(loadKey))
        {
            BeginNewGame();
            storage.Load(this);
        }


        for (int i = 1; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            if (Input.GetKeyDown(keyCodes[i - 1]))
            {
                StartCoroutine(LoadLevel(i));

            }
        }
    }

    public void LoadScene(int levelBuildIndex)
    {
        StartCoroutine(LoadLevel(levelBuildIndex));
    }
    public void LoadNextLevel()
    {
        LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    IEnumerator LoadLevel(int levelBuildIndex)
    {
        enabled = false;
        if (buildIndex > 0)
        {
            yield return SceneManager.UnloadSceneAsync(buildIndex);
        }
        yield return SceneManager.LoadSceneAsync(
            levelBuildIndex, LoadSceneMode.Additive
        );
        Debug.Log("cganging level");
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(levelBuildIndex));
        buildIndex = levelBuildIndex;
        enabled = true;
    }
    public override void Save(GameDataWriter writer)
    {
        //DebugFactoryID();
        writer.Write(Random.state);
        scoreController.Save(writer);
        writer.Write(buildIndex);
        LevelController.current.Save(writer);
        /* writer.Write(enemies.Count);
         foreach (var enemy in enemies)
         {
             writer.Write(enemy.OriginFactory.FactoryID);
             writer.Write(enemy.EnemyID);
             enemy.Save(writer);
         }
         */
    }

    public IEnumerator LoadGame(GameDataReader reader)
    {
        Random.state = reader.ReadRandomState();
        scoreController.Load(reader);

        yield return LoadLevel(reader.ReadInt());
        LevelController.current.Load(reader);
        /* 
        int enemiesLenght = reader.ReadInt();
        for (int i = 0; i < enemiesLenght; i++)
        {
            int factoryId = reader.ReadInt();
            int enemyId = reader.ReadInt();
            //Debug.Log(factoryId);
            Enemy instance = factories[factoryId].Get(enemyId);
            instance.Load(reader);
        }
        */
    }
    public override void Load(GameDataReader reader)
    {
        int version = reader.Version;
        if (version > saveVersion)
        {
            Debug.LogError("Unsopported save file version :" + version);
            return;
        }
        StartCoroutine(LoadGame(reader));

    }
    private void BeginNewGame()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].Recycle();
        }
        enemies.Clear();
        int seed = Random.Range(0, int.MaxValue) ^ (int)Time.unscaledTime;
        Random.InitState(seed);

    }


    public void AddEnemy(Enemy enemy)
    {
        enemies.Add(enemy);
    }
    public void RemoveEnemy(Enemy enemy)
    {
        enemies.Remove(enemy);
    }

    void DebugFactoryID()
    {
        for (int i = 0; i < factories.Length; i++)
        {
            Debug.Log(factories[i].FactoryID);
        }
    }
    void OnEnable()
    {
        if (factories[0].FactoryID != 0)
        {
            for (int i = 0; i < factories.Length; i++)
            {
                factories[i].FactoryID = i;
            }
        }
    }

    public List<AllyController> GetEnemiesByType(EnemyTypes type)
    {
        var enemeyList = new List<AllyController>();
        foreach (var enemy in enemies)
        {
            if (enemy.EType == type)
            {
                enemeyList.Add(enemy as AllyController);
            }
        }
        return enemeyList;

    }
}
