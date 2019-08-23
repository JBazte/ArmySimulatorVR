using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : PersistableObject
{
    public static LevelController current;
    [SerializeField]
    PersistableObject[] objectToSave;


    public override void Save(GameDataWriter writer)
    {
        writer.Write(objectToSave.Length);
        foreach (var o in objectToSave)
        {
            o.Save(writer);
        }
    }
    public void Awake()
    {
        current = this;
    }
    public override void Load(GameDataReader reader)
    {
        int index = reader.ReadInt();
        for (int i = 0; i < index; i++)
        {
            objectToSave[i].Load(reader);
        }
    }


}
