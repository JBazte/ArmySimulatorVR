
using UnityEngine;
using Valve.VR.InteractionSystem;
[RequireComponent(typeof(Interactable))]
public class ItemSpawner : MonoBehaviour
{
    [SerializeField]
    protected GrabableObject item;
    [SerializeField]
    Transform transformPositon;

    [SerializeField]
    private bool attachToHand = false;

    private Interactable interactable;

    private void Start()
    {
        interactable = GetComponent<Interactable>();
        if (transformPositon == null)
        {
            transformPositon = transform;
        }
    }
    public void SpawnObject(Hand hand)
    {
        if(item != null)
        {

        GrabableObject gb = Instantiate(item, hand.transform.position, Quaternion.identity);
        if (attachToHand)
        {
            gb.AttachToHand(hand);
        }
        }
    }
    public void SpawnObject()
    {
        GrabableObject gb = Instantiate(item, transformPositon.position, Quaternion.identity);
    }
    public void SpawnObject(GrabableObject item)
    {

        Instantiate(item, transformPositon.position, Quaternion.identity);
    }
    public void SpawnObject(GrabableObject item, Transform parent)
    {

        Instantiate(item, transformPositon.position, Quaternion.identity, parent);
    }

    private void HandHoverUpdate(Hand hand)
    {
        GrabTypes startingGrabType = hand.GetGrabStarting();
        bool isGrabEnding = hand.IsGrabEnding(this.gameObject);

        if (interactable.attachedToHand == null && startingGrabType != GrabTypes.None)
        {
            SpawnObject(hand);
        }

    }

}
