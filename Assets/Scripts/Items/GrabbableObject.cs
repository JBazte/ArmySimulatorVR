using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;
using Valve.VR;

[RequireComponent(typeof(Interactable))]
public class GrabbableObject : MonoBehaviour
{


    [SerializeField]
    Transform grabposition;
    protected Interactable interactable;

    [SerializeField]
    protected bool isDoubleHanded;

    SecondHanded secondHand;


    Hand grabbingHand;
    public int HandsAttached { get; private set; }
    [EnumFlags]

    public Hand.AttachmentFlags attachmentFlags = Hand.AttachmentFlags.ParentToHand | Hand.AttachmentFlags.DetachFromOtherHand | Hand.AttachmentFlags.DetachOthers | Hand.AttachmentFlags.VelocityMovement;
    [SerializeField]
    public MyGrabTypes grabTypes = MyGrabTypes.Press;
    [SerializeField]
    protected bool restoreParent = true;

    public bool resetRotationOnSocket = true;

    private Socket activeSocket;
    private bool isAvailable = true;
    void Start()
    {

        if (grabposition != null)
        {
            attachmentFlags = Hand.AttachmentFlags.SnapOnAttach | attachmentFlags;
        }
        if (isDoubleHanded)
        {
            secondHand = GetComponentInChildren<SecondHanded>();
        }


    }

    private void OnEnable()
    {
        if (secondHand == null)
        {
            secondHand = GetComponentInChildren<SecondHanded>();
        }
    }
    public void AttachNewSocket(Socket newSocket)
    {
        if (newSocket.GetStoredObject)
            return;
        RealeaseOldSocket();
        activeSocket = newSocket;

        activeSocket.Attach(this);
        isAvailable = false;
        interactable.highlightOnHover = false;

    }

    public void RealeaseOldSocket()
    {
        if (!activeSocket)
            return;

        activeSocket.DetAttach();
        activeSocket = null;
        isAvailable = true;
        interactable.highlightOnHover = true;


    }
    //-------------------------------------------------
    // Called when a Hand starts hovering over this object
    //-------------------------------------------------
    private void OnHandHoverBegin(Hand hand)
    {

    }


    //-------------------------------------------------
    // Called when a Hand stops hovering over this object
    //-------------------------------------------------
    private void OnHandHoverEnd(Hand hand)
    {

    }


    //-------------------------------------------------
    // Called every Update() while a Hand is hovering over this object
    //-------------------------------------------------
    private void HandHoverUpdate(Hand hand)
    {
        /*
        if (isAvailable)
        {

            GrabTypes startingGrabType = hand.GetGrabStarting();
            bool isGrabEnding = hand.IsGrabEnding(this.gameObject);

            if (interactable.attachedToHand == null && startingGrabType != GrabTypes.None)
            {
                if (activeSocket)
                {
                    activeSocket.InteractSlot(hand);
                    return;
                }

                HandsAttached = 1;
                grabbingHand = hand;
                hand.HoverLock(interactable);

                // Attach this object to the hand
                hand.AttachObject(gameObject, startingGrabType, attachmentFlags, grabposition);
            }
            else if (isGrabEnding)
            {
                // Detach this object from the hand
                hand.DetachObject(gameObject, restoreParent);

                // Call this to undo HoverLock
                hand.HoverUnlock(interactable);


            }
         */
        if (isAvailable)
        {
            if (grabTypes == MyGrabTypes.Press)
            {
                if (SteamVR_Input.GetStateDown("Shoot", hand.handType))
                {
                    if (activeSocket)
                    {
                        activeSocket.InteractSlot(hand);
                        return;
                    }
                    if (hand.ObjectIsAttached(this.gameObject))
                    {
                        // Detach this object from the hand
                        hand.DetachObject(gameObject, restoreParent);
                        // Call this to undo HoverLock
                        hand.HoverUnlock(interactable);
                    }
                    else
                    {
                        HandsAttached = 1;
                        grabbingHand = hand;
                        hand.HoverLock(interactable);
                        // Attach this object to the hand
                        hand.AttachObject(gameObject, GrabTypes.Scripted, attachmentFlags, grabposition);
                    }

                }
            }
            else if (grabTypes == MyGrabTypes.Hold)
            {
                if (SteamVR_Input.GetStateUp("Shoot", hand.handType))
                {
                    if (hand.ObjectIsAttached(this.gameObject))
                    {
                        // Detach this object from the hand
                        hand.DetachObject(gameObject, restoreParent);
                        // Call this to undo HoverLock
                        hand.HoverUnlock(interactable);
                    }
                }
                else if (SteamVR_Input.GetStateDown("Shoot", hand.handType))
                {
                    if (!hand.ObjectIsAttached(this.gameObject))
                    {
                        HandsAttached = 1;
                        grabbingHand = hand;
                        hand.HoverLock(interactable);
                        // Attach this object to the hand
                        hand.AttachObject(gameObject, GrabTypes.Scripted, attachmentFlags, grabposition);
                    }
                }

            }
            else
            {
                if (SteamVR_Input.GetStateDown("Shoot", hand.handType))
                {
                    if (!hand.ObjectIsAttached(this.gameObject))
                    {
                        HandsAttached = 1;
                        grabbingHand = hand;
                        hand.HoverLock(interactable);
                        // Attach this object to the hand
                        hand.AttachObject(gameObject, GrabTypes.Scripted, attachmentFlags, grabposition);
                    }
                }
            }
        }
    }


    public void AttachToHand(Hand hand)
    {
        // GrabTypes startingGrabType = hand.GetGrabStarting();
        hand.HoverLock(interactable);
        hand.AttachObject(gameObject, GrabTypes.Scripted, attachmentFlags, grabposition);
        RealeaseOldSocket();
        grabbingHand = hand;
        HandsAttached = 1;
        //hand.HoverLock(interactable);


    }

    //-------------------------------------------------
    // Called when this GameObject becomes attached to the hand
    //-------------------------------------------------
    protected virtual void OnAttachedToHand(Hand hand)
    {
        Debug.Log("Attached to hand");
        if (isDoubleHanded)
        {
            secondHand.StartGrabbable(this);
        }
    }



    //-------------------------------------------------
    // Called when this GameObject is detached from the hand
    //-------------------------------------------------
    protected virtual void OnDetachedFromHand(Hand hand)
    {
        Debug.Log("Detached from hand");
        if (isDoubleHanded)
        {
            secondHand.StopGrabble(hand.otherHand);
        }
    }


    //-------------------------------------------------
    // Called every Update() while this GameObject is attached to the hand
    //-------------------------------------------------
    private void HandAttachedUpdate(Hand hand)
    {


    }


    private void Update()
    {
        if (isDoubleHanded)
        {
            if (Input.GetKeyDown(KeyCode.K))
            {

                //secondHand.StartGrabbable(this);
            }


        }


    }

    public void DoubleHanded()
    {
        HandsAttached = 2;
    }
    public void StopDoubleHanded()
    {
        HandsAttached = 1;
    }


    //-------------------------------------------------
    // Called when this attached GameObject becomes the primary attached object
    //-------------------------------------------------
    private void OnHandFocusAcquired(Hand hand)
    {
    }


    //-------------------------------------------------
    // Called when another attached GameObject becomes the primary attached object
    //-------------------------------------------------
    private void OnHandFocusLost(Hand hand)
    {
    }
}

public enum MyGrabTypes
{
    None,
    Press,
    Hold
}
