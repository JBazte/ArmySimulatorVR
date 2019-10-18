using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrackStats : CharacterStats
{
    protected override void Die()
    {
        if (GameController.instance.OnFinishLevel != null)
            GameController.instance.OnFinishLevel.Invoke();
        base.Die();


        GameController.instance.LoadNextLevel();


    }
}
