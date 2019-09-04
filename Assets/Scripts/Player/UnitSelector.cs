using UnityEngine;
using Valve.VR.InteractionSystem;
using Valve.VR;
public class UnitSelector : MonoBehaviour
{

    private Hand hand;
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
        if (newSelected != null)
        {

            if (!BlockSelection)
            {
                if (selected != null)
                {
                    // Two Step Selection
                    selected.AfterSelected(newSelected);
                    selected.Diselected();
                }
                newSelected.OnSelected();
                this.selected = newSelected;
                if (newSelected != null)
                    Debug.Log(newSelected);
            }
        }
        else
        {
            UnSelect();
        }
    }

    void UnSelect()
    {
        if (selected != null)
            selected.Diselected();
        selected = null;
        //Player.instance.transform.position = startPosition;
        //Player.instance.transform.rotation = startRotation;



    }
}
