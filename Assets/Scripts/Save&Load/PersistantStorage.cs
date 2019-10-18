using UnityEngine;
using System.IO;
public class PersistantStorage : MonoBehaviour
{
    string savePath;
    string highScorePath;

    void Awake()
    {

        savePath = Path.Combine(Application.persistentDataPath, "saveFile");
        highScorePath = Path.Combine(Application.persistentDataPath, "highScores");


    }

    public void Save(PersistableObject o, int version)
    {
        using (

            var writer = new BinaryWriter(File.Open(savePath, FileMode.Create))
        )
        {
            writer.Write(-version);
            o.Save(new GameDataWriter(writer));
        }

    }

    public void Load(PersistableObject o)
    {
        byte[] data = File.ReadAllBytes(savePath);
        var reader = new BinaryReader(new MemoryStream(data));
        o.Load(new GameDataReader(reader, -reader.ReadInt32()));
    }

    public void SaveHighScore(ScoreController o)
    {
        using (

            var writer = new BinaryWriter(File.Open(highScorePath, FileMode.Create))
        )
        {

            o.SaveHighScores(new GameDataWriter(writer));
        }

    }

    public void LoadHighScore(ScoreController o)
    {
        try
        {
            byte[] data = File.ReadAllBytes(highScorePath);
            var reader = new BinaryReader(new MemoryStream(data));
            o.LoadHighScores(new GameDataReader(reader, 1));
            //Debug.Log(highScorePath);

        }
        catch (FileNotFoundException e)
        {
            Debug.LogError("Coudn't Find the correct file: " + e.ToString());
        }
    }
}
