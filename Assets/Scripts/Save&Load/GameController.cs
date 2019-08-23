using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
[DisallowMultipleComponent]
public class GameController : PersistableObject
{
    [SerializeField]
    PersistantStorage storage;
    [SerializeField]
    ScoreController scoreController;
    const int saveVersion = 0;

    [SerializeField]
    private KeyCode saveKey;
    [SerializeField]
    private KeyCode loadKey;

    private Enemy[] enemies;
    private void Awake()
    {
        storage = new PersistantStorage();
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
            storage.Load(this);
        }
    }

    public override void Save(GameDataWriter writer)
    {
        scoreController.Save(writer);
    }

    public override void Load(GameDataReader reader)
    {
        scoreController.Load(reader);
    }
}
