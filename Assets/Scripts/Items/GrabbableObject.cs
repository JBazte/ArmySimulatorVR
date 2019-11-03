﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

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
    [Tooltip("The flags used to attach this object to the hand.")]
    public Hand.AttachmentFlags attachmentFlags = Hand.AttachmentFlags.ParentToHand | Hand.AttachmentFlags.DetachFromOtherHand | Hand.AttachmentFlags.DetachOthers | Hand.AttachmentFlags.VelocityMovement;
    [SerializeField]
    protected bool restoreParent = true;
    void Start()
    {
        interactable = GetComponentInChildren<Interactable>();
        if (grabposition != null)
        {
            attachmentFlags = Hand.AttachmentFlags.SnapOnAttach | attachmentFlags;
        }
        if (isDoubleHanded)
        {
            secondHand = GetComponentInChildren<SecondHanded>();
        }
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
        GrabTypes startingGrabType = hand.GetGrabStarting();
        bool isGrabEnding = hand.IsGrabEnding(this.gameObject);

        if (interactable.attachedToHand == null && startingGrabType != GrabTypes.None)
        {
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
    }

    public void AttachToHand(Hand hand)
    {
        GrabTypes startingGrabType = hand.GetGrabStarting();
        hand.AttachObject(gameObject, startingGrabType, attachmentFlags, grabposition);
        //hand.HoverLock(interactable);


    }

    //-------------------------------------------------
    // Called when this GameObject becomes attached to the hand
    //-------------------------------------------------
    private void OnAttachedToHand(Hand hand)
    {
        if (isDoubleHanded)
        {
            secondHand.StartGrabbable(this);
        }
    }



    //-------------------------------------------------
    // Called when this GameObject is detached from the hand
    //-------------------------------------------------
    private void OnDetachedFromHand(Hand hand)
    {
        if (isDoubleHanded)
        {
            secondHand.StopGrabble();
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

                secondHand.StartGrabbable(this);
            }


        }


    }

    public void DoubleHanded()
    {
        grabbingHand.SetSecondHand(transform);
        HandsAttached = 2;

    }
    public void StopDoubleHanded()
    {
        grabbingHand.RemoveSecondHand();
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
