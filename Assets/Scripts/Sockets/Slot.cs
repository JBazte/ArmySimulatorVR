
using UnityEngine;
using System.Collections;
using Valve.VR.InteractionSystem;
using Valve.VR;
[RequireComponent(typeof(Interactable))]
public class Slot : MonoBehaviour
{
    private Socket socket = null;
    Interactable interactable;
    [SerializeField]
    float InteractRadious;
    [SerializeField]
    Hand[] hands;

    [SerializeField]
    GrabbableObject startingSelectable;
    void Awake()
    {
        socket = GetComponent<Socket>();
        interactable = GetComponent<Interactable>();
    }

    private void Start()
    {
        hands = Player.instance.hands;
        if (startingSelectable != null)
            TryStore(startingSelectable);
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


    }



    private bool isInside;
    private void Update()
    {
        for (int i = 0; i < hands.Length; i++)
        {
            float distance = Vector3.Distance(transform.position, hands[i].transform.position);
            if (distance < InteractRadious)
            {
                if (SteamVR_Input.GetStateUp("Shoot", hands[i].handType))
                {
                    if (hands[i].currentAttachedObject)
                    {
                        if (hands[i].currentAttachedObject.GetComponent<GrabbableObject>().grabTypes == MyGrabTypes.Hold)
                            StartInteraction(hands[i]);
                    }
                }
                else if (SteamVR_Input.GetStateDown("Shoot", hands[i].handType))
                {
                    StartInteraction(hands[i]);
                }
                else if (Input.GetKeyDown(KeyCode.N))
                {
                    StartInteraction(hands[i]);
                }
            }



        }
    }


    private void TryStore(Hand hand)
    {

        if (socket.GetStoredObject)
            return;
        GameObject g = hand.currentAttachedObject;

        GrabbableObject gr = g.GetComponentInChildren<GrabbableObject>();
        gr.AttachNewSocket(socket);
        hand.DetachObject(g, false);
        hand.HoverUnlock(interactable);

        // StartCoroutine(WaitStore(hand));

    }
    public void TryStore(GrabbableObject gb)
    {
        if (socket.GetStoredObject)
            return;
        gb.AttachNewSocket(socket);

    }



    private void TryRetrieve(Hand hand)
    {
        if (!socket.GetStoredObject)
            return;
        GrabbableObject ob = socket.GetStoredObject;
        socket.DetAttach();
        ob.AttachToHand(hand);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, InteractRadious);
    }

    public void DestroyAttachedObject()
    {
        GrabbableObject attachedObject = socket.GetStoredObject;

        if (attachedObject != null)
        {
            Destroy(attachedObject.gameObject);
        }
        socket.DetAttach();
    }

}
