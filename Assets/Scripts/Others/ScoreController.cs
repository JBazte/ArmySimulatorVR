
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

    public System.Action OnScoreChange;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            AddScore(1000);
        }
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
        if (x.score == 0 || y.score == 0)
        {
            return 0;
        }

        // CompareTo() method 
        return y.score.CompareTo(x.score);
    }

    private void SortTopScores()
    {
        topScores.Sort(CompareScores);

    }

    private void AddTopScore(Score score)
    {

        if (topScores.Count >= topScores.Capacity)
        {
            SortTopScores();
            int lastScore = topScores.Count - 1;

            if (topScores[lastScore].score < score.score)
            {
                topScores[lastScore] = score;
            }
        }
        else
        {
            topScores.Add(score);
            SortTopScores();
        }
    }
    public override void Save(GameDataWriter writer)
    {
        AddTopScore(new Score(playerName, score));
        writer.Write(topScores.Count);
        foreach (var score in topScores)
        {
            writer.Write(score);
        }
        writer.Write(score);
    }
    public override void Load(GameDataReader reader)
    {
        int index = reader.ReadInt();
        for (int i = 0; i < index; i++)
        {
            AddTopScore(reader.ReadScore());
        }
        score = reader.ReadFloat();
    }

}
