using UnityEngine;
using System.IO;
public class GameDataWriter
{
    BinaryWriter writer;
    public GameDataWriter(BinaryWriter writer)
    {
        this.writer = writer;
    }

    public void Write(Vector3 data)
    {
        writer.Write(data.x);
        writer.Write(data.y);
        writer.Write(data.z);
    }

    public void Write(Quaternion data)
    {
        writer.Write(data.x);
        writer.Write(data.y);
        writer.Write(data.z);
        writer.Write(data.w);
    }

    public void Write(int data)
    {
        writer.Write(data);
    }

    public void Write(float data)
    {
        writer.Write(data);
    }
    public void Write(string data)
    {
        writer.Write(data);
    }

    public void Write(Random.State value)
    {
        writer.Write(JsonUtility.ToJson(value));
    }
    public void Write(bool data)
    {
        writer.Write(data);
    }

    public void Write(Score score)
    {
        writer.Write(score.name);
        writer.Write(score.score);
    }


}
