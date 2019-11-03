using Valve.VR.InteractionSystem;
using UnityEngine;

[RequireComponent(typeof(Interactable))]
public class SecondHanded : MonoBehaviour
{
    protected Interactable interactable;
    GrabbableObject grabbableObject;
    private bool isGrabbable;
    public void StartGrabbable(GrabbableObject o)
    {
        if (isGrabbable)
        {
            StopGrabble();
            return;
        }

        grabbableObject = o;
        isGrabbable = true;
        interactable.highlightOnHover = true;
    }

    public void StopGrabble()
    {
        isGrabbable = false;
        interactable.highlightOnHover = false;
        grabbableObject.StopDoubleHanded();
    }

    private void Start()
    {
        interactable = GetComponentInChildren<Interactable>();
        interactable.highlightOnHover = false;
    }

    private void HandHoverUpdate(Hand hand)
    {
        if (isGrabbable)
        {

            GrabTypes startingGrabType = hand.GetGrabStarting();
            bool isGrabEnding = hand.IsGrabEnding(this.gameObject);

            if (interactable.attachedToHand == null && startingGrabType != GrabTypes.None)
            {
                grabbableObject.DoubleHanded();

            }
            else if (isGrabEnding)
            {

                StopGrabble();
            }
        }
    }


}
