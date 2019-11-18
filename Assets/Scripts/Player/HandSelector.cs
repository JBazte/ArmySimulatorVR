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
    [SerializeField]
    Transform dot;
    UnitSelector unitSelector;
    [SerializeField]
    float pointerLenght = 50f;

    Camera mainCamera;
    private LineRenderer lineRenderer = null;

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
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                CheckUnit();
            }

        }
        if (!unitSelector.BlockSelection)
        {
            UpdateLine();
        }
        else
        {
            lineRenderer.gameObject.SetActive(false);
        }

    }


    private void UpdateLine()
    {
        lineRenderer.gameObject.SetActive(true);
        //Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        Ray ray = new Ray(transform.position, transform.forward);
        var hit = Raycast(layerMask, ray);
        Vector3 endposition = transform.position + (transform.forward * pointerLenght);
        if (hit.collider != null)
            endposition = hit.point;

        dot.position = endposition;

        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, endposition);

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
            ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        }


        Selectable selected = SphereRaycast<Selectable>(layerMask, ray);
        unitSelector.Select(selected, hand);
    }

    public T SphereRaycast<T>(LayerMask mask, Ray ray) where T : Selectable
    {
        T selected = null;
        RaycastHit hit;
        if (Physics.SphereCast(ray, sphereSelectionRadious, out hit, pointerLenght, layerMask))
        {
            selected = hit.collider.GetComponentInParent<T>();
        }

        return selected;
    }
    public RaycastHit Raycast(LayerMask mask, Ray ray)
    {
        RaycastHit hit;
        Physics.Raycast(ray, out hit, pointerLenght, mask);
        return hit;
    }

    void Start()
    {
        hand = GetComponentInParent<Hand>();
        unitSelector = GameController.instance.unitSelector;
        mainCamera = Player.instance.GetComponentInChildren<Camera>();
        lineRenderer = GetComponentInChildren<LineRenderer>();
    }
}
