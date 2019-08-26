using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : PersistableObject
{
    public static LevelController current;
    [SerializeField]
    PersistableObject[] objectToSave;

    [SerializeField]
    public WaveController waveController;


    public override void Save(GameDataWriter writer)
    {
        waveController.Save(writer);
        writer.Write(objectToSave.Length);
        foreach (var o in objectToSave)
        {
            o.Save(writer);
        }
    }
    public void Awake()
    {
        current = this;
        if (waveController == null)
        {
            waveController = GetComponentInChildren<WaveController>();
        }
    }
    public override void Load(GameDataReader reader)
    {
        waveController.Load(reader);
        int index = reader.ReadInt();
        for (int i = 0; i < index; i++)
        {
            objectToSave[i].Load(reader);
        }
    }


}
