using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barracks : Selectable
{
    [SerializeField]
    Transform standPoint;

    AllyController occupant;


    public Transform GetStandPoint
    {
        get
        {
            return standPoint;
        }
    }
    public override SelectableTypes Type
    {
        get
        {
            return SelectableTypes.Barracks;
        }
    }

    public void Occupy(AllyController newOccupant)
    {
        if (occupant != null)
        {
            occupant.ChangeBarracks(newOccupant);
        }
        else
        {
            newOccupant.SetPrioirityPoint(GetStandPoint.position);
        }
        occupant = newOccupant;
    }

    public void DesOccupy()
    {
        occupant = null;
    }


    public override void Diselected()
    {
        HideRadialMenu();
        var rm = GetMyRadialMenu;
        rm.top.RemoveFunctionsOnPress();
        rm.right.RemoveFunctionsOnPress();
    }


    public override void OnSelected(Valve.VR.InteractionSystem.Hand hand)
    {
        ShowRadialMenu();
        var rm = GetMyRadialMenu;
        rm.top.AddFunctionOnPress(delegate { SetTypeTarget(EnemyTypes.Rifle); });
        rm.right.AddFunctionOnPress(delegate { SetTypeTarget(EnemyTypes.Sniper); });



    }

    public void SetTypeTarget(EnemyTypes enemyType)
    {
        var possibilites = GameController.instance.GetEnemiesByType(enemyType);
        if (possibilites.Count > 0)
        {
            foreach (AllyController ally in possibilites)
            {

                if (ally.GetAvailability)
                {
                    ally.SetBarrack(this);
                    break;
                }
            }
        }
        Diselected();
    }
}
