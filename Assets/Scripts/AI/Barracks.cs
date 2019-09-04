using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barracks : Selectable
{
    [SerializeField]
    Transform standPoint;

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

    public override void Diselected()
    {

    }


    public override void OnSelected()
    {

    }
}
