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
    Selectable selected;


    public bool BlockSelection { get; set; }

    private void Start()
    {
        hand = GetComponentInParent<Hand>();
        startPosition = transform.position;
    }

    void Update()
    {
        if (hand != null)
        {
            // if (SteamVR_Input.GetStateDown(selected.InputAction., hand.handType))
            if (selected.InputAction.GetStateDown(hand.handType))
            {
                selected.OnInputAction();
            }
            if (selected != null)
            {

            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                // Select();
            }
        }


    }

    public void Select(Selectable newSelected)
    {
        if (BlockSelection)
        {
            if (selected != null)
            {
                // Two Step Selection
                selected.AfterSelected(newSelected);
                selected.Diselected();
            }
            selected.OnSelected();
            this.selected = newSelected;
            if (selected != null)
                Debug.Log(selected);
        }
    }

    void UnSelect()
    {
        selected.Diselected();

        Player.instance.transform.position = startPosition;
        Player.instance.transform.rotation = startRotation;
        selected = null;


    }
}
