using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;
using Valve.VR;
public class AllyController : EnemyController
{
    [SerializeField]
    GrabbableObject objectPrefab;
    [SerializeField]
    SteamVR_Input_Sources handType = SteamVR_Input_Sources.RightHand;

    List<Color> defaultColors;
    private GrabbableObject w;
    private Hand hand;
    SkinnedMeshRenderer mr;
    bool isIncarnated;
    Barracks barrack;
    public override SelectableTypes Type
    {
        get
        {
            return SelectableTypes.Ally;
        }
    }


    public void Incarnate()
    {

        Player player = Player.instance;
        Hand hand = player.GetHand(handType);
        if (hand == null)
        {
            hand = player.GetHand(SteamVR_Input_Sources.Any);
        }
        this.hand = hand;
        w = Instantiate(objectPrefab);
        w.transform.position = hand.transform.position;

        w.AttachToHand(hand);

        player.transform.position = transform.position;
        player.transform.rotation = transform.rotation;
        player.GetComponentInChildren<MagazineSpawner>(true).ChangeType((w.GetComponentInChildren<Weapon>()).MagazineType);
        gameObject.SetActive(false);
        isIncarnated = true;

    }

    public void DisIncarnate()
    {
        gameObject.SetActive(true);
        if (w != null)
        {
            hand.DetachObject(w.gameObject, false);
            hand.otherHand.DetachObject(hand.currentAttachedObject, false);
            Destroy(w.gameObject);
        }
        isIncarnated = false;

    }
    public override void SetPoint(Vector3 position)
    {
        if (!priorityMoving)
        {
            base.SetPoint(position);

        }
    }

    public void SetPrioirityPoint(Vector3 position)
    {
        priorityMoving = false;
        SetPoint(position);
        priorityMoving = true;
    }
    private void Start()
    {
        MeshRenderer[] ms = GetComponentsInChildren<MeshRenderer>();
        foreach (var m in ms)
        {
            defaultColors.Add(m.material.color);
        }
        mr = GetComponentInChildren<SkinnedMeshRenderer>();
        base.Start();
    }
    public void ChangeColor(Color color)
    {
        MeshRenderer[] ms = GetComponentsInChildren<MeshRenderer>();
        foreach (var m in ms)
        {
            m.material.color = color;
        }
    }
    public void ChangeSpecificColor(Color color)
    {
        mr.materials[8].color = color;
    }

    public void ResetSpecificColor()
    {
        mr.materials[8].color = Color.yellow;
    }

    public void ResetColors()
    {
        MeshRenderer[] ms = GetComponentsInChildren<MeshRenderer>();

        for (int i = 0; i < defaultColors.Count; i++)
        {
            ms[i].material.color = defaultColors[i];
        }
    }

    public override void OnSelected()
    {
        ChangeSpecificColor(Color.green);

    }
    public override void Diselected()
    {
        ChangeSpecificColor(Color.yellow);
    }
    public override void AfterSelected(Selectable selectable)
    {
        if (selectable.Type == SelectableTypes.Barracks)
        {
            Barracks newBarrack = selectable as Barracks;
            if (barrack != null)
                barrack.DesOccupy();

            newBarrack.Occupy(this);

            barrack = newBarrack;
            //SetPoint(b.GetStandPoint.position);

        }
    }

    public void ChangeBarracks(AllyController other)
    {
        barrack = other.barrack;
        ChangeTarget(other);
        if (other.barrack != null)
            other.barrack.Occupy(this);

    }

    public override void OnInputAction(UnitSelector selector)
    {
        Debug.Log("incarnating");
        if (!isIncarnated)
        {
            selector.BlockSelection = true;
            Incarnate();
        }
        else
        {
            selector.BlockSelection = false;
            DisIncarnate();
            selector.ResetPlayerPositon();

        }

    }

}
