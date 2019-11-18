using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class InputManager : MonoBehaviour
{
    [Header("Actions")]
    public SteamVR_Action_Boolean touch = null;
    public SteamVR_Action_Boolean press = null;
    public SteamVR_Action_Vector2 touchPosition = null;

    [Header("Scene Objects")]
    public RadialMenu[] radialMenus = null;

    private void Awake()
    {
        touch.onChange += Touch;
        press.onStateUp += PressRelease;
        touchPosition.onAxis += Position;
    }

    private void Start()
    {
        radialMenus = UnitSelector.radialInstances;
        
    }

    private void OnDestroy()
    {
        touch.onChange -= Touch;
        press.onStateUp -= PressRelease;
        touchPosition.onAxis -= Position;
    }

    private void Position(SteamVR_Action_Vector2 fromAction, SteamVR_Input_Sources fromSource, Vector2 axis, Vector2 delta)
    {
        foreach (RadialMenu rm in radialMenus)
            rm.SetTouchPosition(axis);
    }

    private void Touch(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
    {
        foreach (RadialMenu rm in radialMenus)
            rm.Show(newState);
    }

    private void PressRelease(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        foreach (RadialMenu rm in radialMenus)
            rm.ActivateHighlightedSection();
    }
}