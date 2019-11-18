using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class RadialSection
{
    public Sprite icon = null;
    public SpriteRenderer iconRenderer = null;
    public UnityEvent onPress = new UnityEvent();

    public void AddFunctionOnPress(UnityAction action)
    {
        onPress.AddListener(action);

    }

    public void RemoveFuntionOnPress(UnityAction action)
    {
        onPress.RemoveListener(action);

    }

    public void RemoveFunctionsOnPress()
    {
        onPress.RemoveAllListeners();

    }
}
