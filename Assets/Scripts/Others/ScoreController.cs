
using UnityEngine;

public class ScoreController : MonoBehaviour
{
    #region Singelton

    public static ScoreController instance;
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

    private float score;

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

}
