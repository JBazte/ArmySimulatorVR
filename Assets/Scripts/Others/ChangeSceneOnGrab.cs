using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

[RequireComponent(typeof(Interactable))]
public class ChangeSceneOnGrab : MonoBehaviour
{
    private void OnAttachedToHand(Hand hand)
    {
        hand.DetachObject(gameObject, false);
        GameController.instance.LoadNextLevel();
        Destroy(gameObject);
    }


}
