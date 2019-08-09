using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class GameDataReader
{
    public int Version { get; }
    private BinaryReader reader;

    public GameDataReader(BinaryReader reader, int version)
    {
        this.reader = reader;
        Version = version;
    }

    public int ReadInt()
    {
        return reader.ReadInt32();
    }
    public float ReadFloat()
    {
        return reader.ReadSingle();
    }
    public Vector3 ReadVector3()
    {
        Vector3 data;
        data.x = reader.ReadSingle();
        data.y = reader.ReadSingle();
        data.z = reader.ReadSingle();
        return data;
    }
    public Quaternion ReadQuaternion()
    {
        Quaternion data = Quaternion.identity;
        data.x = reader.ReadSingle();
        data.y = reader.ReadSingle();
        data.z = reader.ReadSingle();
        data.x = reader.ReadSingle();
        return data;
    }
    public string ReadString()
    {

        return reader.ReadString();

    }
}
