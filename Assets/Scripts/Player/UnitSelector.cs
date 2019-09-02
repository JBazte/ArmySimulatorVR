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
    private Quaternion startRotation;
    private bool isIncarnated;
    AllyController selected;


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
        selected.ResetSpecificColor();
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
                    if (hand.currentAttachedObjectInfo == null)
                        Select();
                }
            }
            if (selected != null)
            {
                if (SteamVR_Input.GetStateDown("Teleport", hand.handType))
                {
                    if (!isIncarnated)
                    {
                        Incarnate(selected);
                    }
                    else
                    {
                        DisCarnate();
                        UnSelect();
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

    void Incarnate(AllyController ally)
    {
        selected.Incarnate();
        isIncarnated = true;
    }

    void DisCarnate()
    {
        selected.DisIncarnate();
        isIncarnated = false;
    }

    public void Select(AllyController ally)
    {
        if (selected != null)
        {

        }
        selected = ally;
        selected.ChangeSpecificColor(Color.green);
        if (selected != null)
            Debug.Log(selected);
    }

    void UnSelect()
    {
        selected.ResetSpecificColor();
        Player.instance.transform.position = startPosition;
        Player.instance.transform.rotation = startRotation;
        selected = null;
        selected = null;

    }
}
