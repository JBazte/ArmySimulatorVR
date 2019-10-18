
using UnityEngine;
using System.Collections.Generic;
public class ScoreController : PersistableObject
{
    #region Singelton
    [SerializeField] string playerName;
    private float score;
    List<Score> topScores = new List<Score>(10);

    public static ScoreController instance;
    public int lastID;
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Trying to creare Two ScoreControllers");
            return;
        }
        instance = this;
    }

    #endregion
    PersistantStorage storage;

    public System.Action OnScoreChange;
    private void Start()
    {
        storage = GameController.instance.Storage;
        GameController.instance.OnFinishLevel += AddCurrentScore;
        storage.LoadHighScore(this);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            AddScore(1000);
        }
        else if (Input.GetKeyDown(KeyCode.J))
        {
            AddTopScore(new Score(playerName, score));
        }


    }
    private void AddCurrentScore()
    {
        Debug.Log("Adding current score");
        AddTopScore(new Score(playerName, score));
    }
    public void AddScore(float amount)
    {
        if (amount > 0)
        {
            score += amount;
            if (OnScoreChange != null)
            {
                OnScoreChange.Invoke();
            }
        }
    }

    public int CompareScores(Score x, Score y)
    {

        // CompareTo() method 
        return y.score.CompareTo(x.score);
    }

    private void SortTopScores()
    {
        topScores.Sort(CompareScores);

    }

    public string TopScoresString
    {
        get
        {
            string result = "";
            foreach (var s in topScores)
            {
                result += s + "\n";
            }

            return result;


        }
    }
    private void AddTopScore(Score score)
    {

        if (topScores.Count >= topScores.Capacity)
        {

            SortTopScores();

            int lastScore = topScores.Count - 1;
            Debug.Log(topScores[lastScore]);
            if (topScores[lastScore].score < score.score)
            {
                topScores[lastScore] = score;
            }
            SortTopScores();
            storage.SaveHighScore(this);
        }
        else
        {
            topScores.Add(score);
            SortTopScores();
            storage.SaveHighScore(this);
        }
    }
    public override void Save(GameDataWriter writer)
    {
        //AddTopScore(new Score(playerName, score));

        writer.Write(score);
        writer.Write(playerName);
    }
    public override void Load(GameDataReader reader)
    {
        score = reader.ReadFloat();
        playerName = reader.ReadString();
    }

    public void SaveHighScores(GameDataWriter writer)
    {
        writer.Write(topScores.Count);
        foreach (var score in topScores)
        {
            writer.Write(score);
        }
    }
    public void LoadHighScores(GameDataReader reader)
    {
        int index = reader.ReadInt();
        for (int i = 0; i < index; i++)
        {
            AddTopScore(reader.ReadScore());
        }
        Debug.Log(TopScoresString);
    }
}
