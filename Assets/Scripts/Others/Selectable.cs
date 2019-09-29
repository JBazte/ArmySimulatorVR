using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
public abstract class Selectable : PersistableObject
{
    public abstract void OnSelected();

    public abstract void Diselected();
    //abstract void OnInteract();

    public virtual void AfterSelected(Selectable selectable)
    {

    }

    public virtual void OnInputAction(UnitSelector selector = null)
    {

    }
    public abstract SelectableTypes Type { get; }
    public SteamVR_Action_Boolean InputAction;

}

public enum SelectableTypes
{
    Ally,
    Enemy,
    Barracks
}




