using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Socket : MonoBehaviour
{
    private GrabbableObject storedObject = null;
    private FixedJoint joint = null;

    Slot slot;
    private void Awake()
    {
        joint = GetComponent<FixedJoint>();
        slot = GetComponent<Slot>();
    }

    public void Attach(GrabbableObject newObject)
    {
        if (storedObject)
            return;
        storedObject = newObject;

        storedObject.transform.position = transform.position;
        storedObject.transform.rotation = Quaternion.identity;

        Rigidbody targetBody = storedObject.gameObject.GetComponent<Rigidbody>();
        joint.connectedBody = targetBody;
    }
    public void InteractSlot(Valve.VR.InteractionSystem.Hand hand)
    {
        if (!slot)
            slot.StartInteraction(hand);

    }

    public void DetAttach()
    {
        if (!storedObject)
            return;

        joint.connectedBody = null;
        storedObject = null;
    }

    public GrabbableObject GetStoredObject
    {
        get
        {
            return storedObject;
        }
    }
}
