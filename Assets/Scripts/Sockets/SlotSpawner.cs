using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;
using Valve.VR;

[RequireComponent(typeof(Interactable))]
public class SlotSpawner : MonoBehaviour
{
    [SerializeField]
    GrabbableObject item;
    Interactable interactable;

    private void Awake()
    {
        interactable = GetComponent<Interactable>();
    }

    private void HandHoverUpdate(Hand hand)
    {
        if (SteamVR_Input.GetStateDown("Shoot", hand.handType))
        {
            SpawnItem(hand);
        }
    }

    public void ChangeSpawn(GrabbableObject newItem)
    {
        item = newItem;
    }

    private void SpawnItem(Hand h)
    {

        GrabbableObject ob = Instantiate(item, h.transform.position, h.transform.rotation);
        ob.AttachToHand(h);
    }
}
