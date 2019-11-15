using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
public abstract class Selectable : PersistableObject
{
    [SerializeField]
    protected int myRadialMenuIndex = 0;
    protected RadialMenu GetMyRadialMenu
    {
        get
        {
            return UnitSelector.GetRadialMenus(myRadialMenuIndex);
        }
    }

    public abstract void OnSelected(Valve.VR.InteractionSystem.Hand hand);

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

    protected void ShowRadialMenu()
    {
        var rm = GetMyRadialMenu;
        if (rm != null)
        {
            rm.Show(true);
            rm.transform.SetParent(transform);
            rm.transform.position = transform.position + Vector3.up * 3;
        }
    }
    protected void HideRadialMenu()
    {
        var rm = GetMyRadialMenu;
        if (rm != null)
        {
            rm.Show(false);
        }
    }

}

public enum SelectableTypes
{
    Ally,
    Enemy,
    Barracks,
    Medic,
    Engenieer
}




