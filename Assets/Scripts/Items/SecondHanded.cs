using Valve.VR.InteractionSystem;
using UnityEngine;

[RequireComponent(typeof(Interactable))]
public class SecondHanded : MonoBehaviour
{
    protected Interactable interactable;

    private bool isGrabbable;
    Hand lockedHand;
    GrabbableObject grabbableObject;
    Hand.AttachmentFlags attachmentFlags = Hand.AttachmentFlags.SecondHand;
    public void StartGrabbable(GrabbableObject o)
    {

        isGrabbable = true;
        interactable.highlightOnHover = true;
        grabbableObject = o;
    }

    public void StopGrabble(Hand hand)
    {
        isGrabbable = false;
        interactable.highlightOnHover = false;
        //grabbableObject.StopDoubleHanded();
        Detach(hand);

    }

    private void Detach(Hand hand)
    {
        if (lockedHand != null)
        {
            lockedHand.HoverUnlock(interactable);
            hand.DetachObject(this.gameObject);
            grabbableObject.StopDoubleHanded();
        }
    }

    private void OnEnable()
    {
        interactable = GetComponentInChildren<Interactable>();
        interactable.highlightOnHover = false;
    }

    private void HandHoverUpdate(Hand hand)
    {
        if (isGrabbable)
        {

            if (Valve.VR.SteamVR_Input.GetStateDown("Shoot", hand.handType))
            {
                if (!hand.ObjectIsAttached(this.gameObject))
                {
                    //grabbableObject.DoubleHanded();
                    hand.HoverLock(interactable);
                    hand.AttachObject(this.gameObject, GrabTypes.Scripted, attachmentFlags);
                    lockedHand = hand;
                    grabbableObject.DoubleHanded();
                }
                else
                {
                    Detach(hand);
                }
            }

        }

    }


}
