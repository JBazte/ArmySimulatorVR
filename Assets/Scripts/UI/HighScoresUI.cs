using TMPro;
using UnityEngine;

public class HighScoresUI : MonoBehaviour
{
    ScoreController scoreController;
    [SerializeField]
    TextMeshProUGUI text;
    void Start()
    {
        scoreController = ScoreController.instance;
        UpdateText();
    }

    void UpdateText()
    {
        text.text = scoreController.TopScoresString;
        Debug.Log("assd");
    }


}
