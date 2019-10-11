
[System.Serializable]
public class Score
{

    public Score(string name, float score, int id)
    {
        this.name = name;
        this.score = score;
        Id = id;
    }
    public string name;
    public float score;
    public int id = int.MinValue;
    public int Id
    {
        get
        {
            return id;
        }
        set
        {
            if (id == int.MinValue && value != int.MinValue)
            {
                id = value;
            }
        }
    }
}
