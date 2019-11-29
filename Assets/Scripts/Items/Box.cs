using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;
using Valve.VR;

public class Box : Selectable
{
    [SerializeField]
    Decoy objectToPlace;
    [SerializeField]
    float rotationSpeed = 2f;

    Decoy objInstance;
    HandSelector handSelector;
    private Hand selectedHand;
    Collider buildingCollider;
    Material buildingMat;
    bool isBuilding;
    [SerializeField]
    private SteamVR_Action_Vector2 moveAction = SteamVR_Input.GetAction<SteamVR_Action_Vector2>("default", "MenuSelectionPosition");

    [SerializeField]
    private LayerMask groundMask;
    [SerializeField]
    private LayerMask nonBuildableMask;

    public override SelectableTypes Type
    {
        get
        {
            return SelectableTypes.Box;
        }

    }

    bool isObjectPlacing;

    public override void Diselected()
    {
        isBuilding = false;
        if (objInstance != null)
        {
            Destroy(objInstance.gameObject);
        }
    }

    public override void OnSelected(Hand hand)
    {
        isBuilding = true;
        objInstance = Instantiate(objectToPlace);
        buildingCollider = objInstance.GetComponentInChildren<Collider>();
        handSelector = hand.GetComponentInChildren<HandSelector>();
        selectedHand = hand;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        UpdatePosition();
    }

    private void ChangeAllColors(Color color)
    {
        var mats = objInstance.GetComponentInChildren<MeshRenderer>().materials;
        foreach (var mat in mats)
        {
            mat.SetColor("_Color", color);
        }
    }

    private void UpdatePosition()
    {
        if (isBuilding)
        {
            bool canBuild;
            var rayHit = handSelector.Raycast(groundMask, new Ray(handSelector.transform.position, handSelector.transform.forward));
            objInstance.transform.position = rayHit.point;
            var touchPosition = moveAction.GetAxis(selectedHand.handType);
            objInstance.transform.Rotate(0f, -touchPosition.x * rotationSpeed, 0f);
            if (Physics.CheckBox(buildingCollider.bounds.center, buildingCollider.bounds.size, Quaternion.identity, nonBuildableMask))
            {
                ChangeAllColors(Color.red);
                canBuild = false;
            }
            else
            {
                ChangeAllColors(Color.green);
                canBuild = true;
            }

            if (Valve.VR.SteamVR_Input.GetStateDown("Teleport", selectedHand.handType))
            {

                if (canBuild)
                {
                    if (objInstance.CallConstruct())
                    {
                        Destroy(gameObject);
                    }

                    Diselected();
                }
                else
                {
                    Diselected();

                }
            }
        }


    }
}
