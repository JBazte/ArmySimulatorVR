using UnityEngine;
using Valve.VR.InteractionSystem;
using Valve.VR;
public class UnitSelector : MonoBehaviour
{
    private Hand hand;
    [SerializeField]
    float sphereSelectionRadious = 2.5f;
    [SerializeField]
    LayerMask layerMask;

    public LayerMask barrackMask;
    AllyController selected;
    private void Select()
    {
        if (selected != null)
        {
            Move();
            return;
        }
        Ray ray;
        if (hand != null)
        {
            ray = new Ray(hand.transform.position, hand.transform.forward);
        }
        else
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        }
        RaycastHit hit;

        if (Physics.SphereCast(ray, sphereSelectionRadious, out hit, 50f, layerMask))
        {
            selected = hit.collider.GetComponentInParent<AllyController>();
            if (selected != null)
                Debug.Log(selected);
        }
    }

    public void Move()
    {
        Ray ray;
        if (hand != null)
        {
            ray = new Ray(hand.transform.position, hand.transform.forward);
        }
        else
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        }
        RaycastHit hit;

        if (Physics.SphereCast(ray, sphereSelectionRadious, out hit, 50f, barrackMask))
        {
            Barracks b = hit.collider.GetComponentInParent<Barracks>();
            if (b != null)
            {
                Transform point = b.GetStandPoint();
                selected.SetPoint(point.position);
            }
        }

        selected = null;
    }

    private void Start()
    {
        hand = GetComponentInParent<Hand>();
    }

    void Update()
    {
        if (hand != null)
        {
            if (SteamVR_Input.GetState("Shoot", hand.handType))
            {
                Select();
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                Select();
            }
        }
    }
}
