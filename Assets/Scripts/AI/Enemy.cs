﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : PersistableObject
{
    public int EnemyID
    {
        get
        {
            return enemyID;
        }
        set
        {
            if (enemyID == int.MinValue && value != int.MinValue)
            {
                enemyID = value;
            }
        }
    }

    public EnemyFactory OriginFactory
    {
        get
        {
            return originFactory;
        }

        set
        {
            if (originFactory == null)
            {
                originFactory = value;
            }
            else
            {
                Debug.LogError("Can't Set Two different Factories to an Enemy");
            }
        }

    }

    private int enemyID = int.MinValue;
    private EnemyFactory originFactory;
    public void Recycle()
    {
        originFactory.Reclaim(this);
    }



}