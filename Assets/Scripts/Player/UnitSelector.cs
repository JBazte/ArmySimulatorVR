using UnityEngine;
using Valve.VR.InteractionSystem;
using Valve.VR;
public class UnitSelector : MonoBehaviour
{
    public static UnitSelector instance;
    private void Awake()
    {
        instance = this;
    }
    private Hand[] hands;
    private Vector3 startPosition;
    private Quaternion startRotation;
    private bool isIncarnated;
    Selectable selected;

    Player player;
    public bool moveSolider;
    public bool BlockSelection { get; set; }
    [SerializeField]
    private RadialMenu[] radialMenus;
    public static RadialMenu[] radialInstances;

    public AllyController allyMove;

    public static RadialMenu GetRadialMenus(int index)
    {
        if (index < 0)
            return null;
        if (index > radialInstances.Length)
            return null;
        return radialInstances[index];

    }



    private void Start()
    {

        player = Player.instance;
        hands = player.hands;
        startPosition = player.transform.position;
        startRotation = player.transform.rotation;

        radialInstances = new RadialMenu[radialMenus.Length];
        for (int i = 0; i < radialMenus.Length; i++)
        {
            radialInstances[i] = Instantiate(radialMenus[i]);
        }

    }

    void Update()
    {
        if (hands != null)
        {
            // if (SteamVR_Input.GetStateDown(selected.InputAction., hand.handType))
            if (selected != null)
            {
                foreach (var hand in hands)
                {
                    //if(selected.InputAction != SteamVR_Action_Boolean.)
                    //if (selected.InputAction.GetStateDown(hand.handType))
                    if (SteamVR_Input.GetStateDown("Teleport", hand.handType))
                    {
                        selected.OnInputAction(this);

                        return;
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
    }

    public void Select(Selectable newSelected, Hand selectedHand)
    {
        if (!BlockSelection)
        {
            if (newSelected != null)
            {

                if (selected != null)
                {
                    // Two Step Selection
                    selected.AfterSelected(newSelected);
                    selected.Diselected();
                }
                newSelected.OnSelected(selectedHand);
                this.selected = newSelected;
                // if (newSelected != null)
                //Debug.Log(newSelected);
            }
            else
            {
                UnSelect();
            }
        }

        AsignPosition();
    }

    public void AsignPosition()
    {
        if (allyMove != null)
        {

            foreach (var hand in hands)
            {
                if (SteamVR_Input.GetStateDown("Shoot", hand.handType))
                {
                    HandSelector hs = hand.GetComponentInChildren<HandSelector>();
                    if (hs != null)
                    {

                        Ray ray = new Ray(hs.transform.position, hs.transform.forward);
                        RaycastHit hit;
                        if (Physics.Raycast(ray, out hit, 50f))
                        {
                            Debug.Log(hit.transform.gameObject);
                            allyMove.SetPoint(hit.point);
                        }

                        allyMove = null;

                    }
                }
            }

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
