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

    Player player;
    public bool BlockSelection { get; set; }

    private void Start()
    {
        hand = GetComponentInParent<Hand>();

        player = Player.instance;
        startPosition = player.transform.position;
        startRotation = player.transform.rotation;

    }
    void Update()
    {
        if (hand != null)
        {
            // if (SteamVR_Input.GetStateDown(selected.InputAction., hand.handType))
            if (selected != null)
            {
                if (selected.InputAction.GetStateDown(hand.handType))
                {
                    selected.OnInputAction(this);
                }

            }
        }
        else
        {
            if (selected != null)
            {
                if (Input.GetKeyDown(KeyCode.N))
                {
                    selected.OnInputAction(this);
                }
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
    public void ResetPlayerPositon()
    {
        Player.instance.transform.position = startPosition;
        Player.instance.transform.rotation = startRotation;
        selected.Diselected();
        selected = null;
    }
}
