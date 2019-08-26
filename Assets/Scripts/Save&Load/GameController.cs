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
    [SerializeField]
    ScoreController scoreController;
    const int saveVersion = 0;

    [SerializeField]
    private KeyCode saveKey;
    [SerializeField]
    private KeyCode loadKey;
    [SerializeField]
    EnemyFactory[] factories;
    private List<Enemy> enemies;
    private int buildIndex;
    private void Awake()
    {
        instance = this;
        enemies = new List<Enemy>();
        storage = new PersistantStorage();
        buildIndex = SceneManager.GetActiveScene().buildIndex;
        if (storage == null)
        {
            storage = FindObjectOfType<PersistantStorage>();
        }
        if (scoreController == null)
        {
            scoreController = FindObjectOfType<ScoreController>();
        }
    }

    private void Update()
    {

        if (Input.GetKeyDown(saveKey))
        {
            Debug.Log("a");
            storage.Save(this, saveVersion);
        }
        if (Input.GetKeyDown(loadKey))
        {
            BeginNewGame();
            storage.Load(this);
        }
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
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(levelBuildIndex));
        buildIndex = levelBuildIndex;
        enabled = true;
    }
    public override void Save(GameDataWriter writer)
    {
        writer.Write(Random.state);
        scoreController.Save(writer);
        writer.Write(buildIndex);
        LevelController.current.Save(writer);
        writer.Write(enemies.Count);
        foreach (var enemy in enemies)
        {
            writer.Write(enemy.OriginFactory.FactoryID);
            writer.Write(enemy.EnemyID);
            enemy.Save(writer);
        }
    }

    public IEnumerator LoadGame(GameDataReader reader)
    {
        Random.state = reader.ReadRandomState();
        scoreController.Load(reader);

        yield return LoadLevel(reader.ReadInt());
        LevelController.current.Load(reader);
        int enemiesLenght = reader.ReadInt();
        for (int i = 0; i < enemiesLenght; i++)
        {
            int factoryId = reader.ReadInt();
            int enemyId = reader.ReadInt();
            Enemy instance = factories[factoryId].Get(enemyId);
            instance.Load(reader);
        }
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
}
