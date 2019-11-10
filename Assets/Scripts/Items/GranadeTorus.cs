using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;
public class GranadeTorus : MonoBehaviour
{
    [SerializeField]
    Granade granade;
    private void Start()
    {
        if (granade == null)
            granade = GetComponentInParent<Granade>();
    }
    private void OnAttachedToHand(Hand hand)
    {
        granade.hasTorus = false;
    }
    private void OnDetachedFromHand(Hand hand)
    {
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<Collider>().isTrigger = false;
    }
}
