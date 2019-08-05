using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats
{
    [SerializeField]
    private float score = 100f;

    protected override void Die()
    {
        ScoreController.instance.AddScore(score);

        base.Die();
    }
}
