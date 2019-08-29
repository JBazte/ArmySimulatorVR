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
    private Vector3 startPosition;
    private bool isIncarnated;
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
            ray = new Ray(transform.position, transform.forward);
        }
        else
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        }
        RaycastHit hit;

        if (Physics.SphereCast(ray, sphereSelectionRadious, out hit, 50f, layerMask))
        {
            selected = hit.collider.GetComponentInParent<AllyController>();
            selected.ChangeColor(Color.green);
            if (selected != null)
                Debug.Log(selected);
        }
    }

    public void Move()
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
        RaycastHit hit;

        if (Physics.SphereCast(ray, sphereSelectionRadious, out hit, 50f, barrackMask))
        {
            Barracks b = hit.collider.GetComponentInParent<Barracks>();
            if (b != null)
            {
                Transform point = b.GetStandPoint();
                selected.SetPoint(point.position);
                Debug.Log(b);
            }
        }
        selected.ChangeColor(Color.blue);
        selected = null;
    }

    private void Start()
    {
        hand = GetComponentInParent<Hand>();
        startPosition = transform.position;
    }

    void Update()
    {
        if (hand != null)
        {
            if (SteamVR_Input.GetStateDown("Shoot", hand.handType))
            {
                if (!isIncarnated)
                {
                    Select();
                }
            }
            if (selected != null)
            {
                if (SteamVR_Input.GetStateDown("Teleport", hand.handType))
                {
                    if (!isIncarnated)
                    {
                        selected.Incarnate();
                        isIncarnated = true;
                    }
                    else
                    {
                        isIncarnated = false;
                        selected.DisIncarnate();
                        selected = null;
                        selected.ChangeColor(Color.blue);
                        transform.position = startPosition;
                    }
                }
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
