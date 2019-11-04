
using UnityEngine;
using Valve.VR.InteractionSystem;

[RequireComponent(typeof(Interactable))]
public class Slot : MonoBehaviour
{
    private Socket socket = null;
    Interactable interactable;

    void Awake()
    {
        socket = GetComponent<Socket>();
        interactable = GetComponent<Interactable>();
    }

    public void StartInteraction(Hand hand)
    {
        if (hand.currentAttachedObject)
        {
            TryStore(hand);
        }
        else
        {
            TryRetrieve(hand);
        }

        Debug.Log("Trying to interact");
    }

    private void OnHandHoverBegin(Hand hand)
    {
        StartInteraction(hand);

    }


    private void TryStore(Hand hand)
    {

        if (socket.GetStoredObject)
            return;
        GameObject g = hand.currentAttachedObject;
        hand.DetachObject(g, false);
        GrabbableObject gr = g.GetComponentInChildren<GrabbableObject>();
        gr.AttachNewSocket(socket);

    }

    private void TryRetrieve(Hand hand)
    {
        if (!socket.GetStoredObject)
            return;
        GrabbableObject ob = socket.GetStoredObject;
        hand.AttachObject(ob.gameObject, ob.grabTypes, ob.attachmentFlags);
    }



}
