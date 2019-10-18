
[System.Serializable]
public class Score
{

    public Score(string name, float score)
    {
        this.name = name;
        this.score = score;

    }
    public string name;
    public float score;

    public override string ToString()
    {
        return name + " " + score;
    }

}
