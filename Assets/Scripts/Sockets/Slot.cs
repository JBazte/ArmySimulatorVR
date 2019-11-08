
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
    void Awake()
    {
        socket = GetComponent<Socket>();
        interactable = GetComponent<Interactable>();
    }

    private void Start()
    {
        hands = Player.instance.hands;
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


    private void OnHoverUpdate(Hand hand)
    {
        if (SteamVR_Input.GetStateDown("Shoot", hand.handType))
        {
            StartInteraction(hand);
        }
        else if (Input.GetKeyDown(KeyCode.N))
        {
            StartInteraction(hand);
        }
    }


    private void Update()
    {
        for (int i = 0; i < hands.Length; i++)
        {
            float distance = Vector3.Distance(transform.position, hands[i].transform.position);
            if (distance < InteractRadious)
            {

                if (SteamVR_Input.GetStateDown("Shoot", hands[i].handType))
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

    IEnumerator WaitStore(Hand hand)
    {
        yield return new WaitForSeconds(.5f);
        GameObject g = hand.currentAttachedObject;
        hand.DetachObject(g, false);
        hand.HoverUnlock(interactable);
    }

    private void TryRetrieve(Hand hand)
    {
        if (!socket.GetStoredObject)
            return;
        GrabbableObject ob = socket.GetStoredObject;
        socket.DetAttach();
        ob.AttachToHand(hand);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, InteractRadious);
    }

}
