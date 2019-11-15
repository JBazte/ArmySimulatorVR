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
    }


    public override void OnSelected(Valve.VR.InteractionSystem.Hand hand)
    {
        ShowRadialMenu();


    }
}
