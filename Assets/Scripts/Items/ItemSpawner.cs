
using UnityEngine;
using Valve.VR.InteractionSystem;

[RequireComponent(typeof(Interactable))]
public class ItemSpawner : MonoBehaviour
{
    [SerializeField]
    protected GrabbableObject item;
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
        if (item != null)
        {

            GrabbableObject gb = Instantiate(item, hand.transform.position, Quaternion.identity);
            if (attachToHand)
            {
                gb.AttachToHand(hand);
            }
        }
    }
    public void SpawnObject()
    {
        GrabbableObject gb = Instantiate(item, transformPositon.position, Quaternion.identity);
    }
    public void SpawnObject(GrabbableObject item)
    {

        Instantiate(item, transformPositon.position, Quaternion.identity);
    }
    public void SpawnObject(GrabbableObject item, Transform parent)
    {

        Instantiate(item, transformPositon.position, Quaternion.identity, parent);
    }

    private void HandHoverUpdate(Hand hand)
    {
        if (Valve.VR.SteamVR_Input.GetStateDown("Shoot", hand.handType))
        {
            SpawnObject(hand);
        }

    }

}
