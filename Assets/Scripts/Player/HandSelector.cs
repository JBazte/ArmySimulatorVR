using Valve.VR.InteractionSystem;
using Valve.VR;
using UnityEngine;

public class HandSelector : MonoBehaviour
{
    private Hand hand;
    [SerializeField]
    private float sphereSelectionRadious;
    [SerializeField]
    LayerMask layerMask;
    UnitSelector unitSelector;
    void Update()
    {
        if (hand != null)
        {
            if (SteamVR_Input.GetStateDown("Shoot", hand.handType))
            {
                if (hand.currentAttachedObjectInfo == null)
                {
                    CheckUnit();
                }
            }
        }
    }

    void CheckUnit()
    {
        Ray ray;
        if (hand != null)
        {
            ray = new Ray(transform.position, transform.forward);
        }
        else
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        }


        Selectable selected = Raycast<Selectable>(layerMask, ray);
        if (selected != null)
        {
            unitSelector.Select(selected);
        }
    }

    public T Raycast<T>(LayerMask mask, Ray ray) where T : Selectable
    {
        T selected = null;
        RaycastHit hit;
        if (Physics.SphereCast(ray, sphereSelectionRadious, out hit, 50f, layerMask))
        {
            selected = hit.collider.GetComponentInParent<T>();
        }
        return selected;
    }

    void Start()
    {
        hand = GetComponentInParent<Hand>();
        unitSelector = GameController.instance.unitSelector;
    }
}
