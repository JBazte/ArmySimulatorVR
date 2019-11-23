using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;
using Valve.VR;
public class AllyController : EnemyController
{
    [SerializeField]
    public Belt.BeltPrefabs configuration;
    [SerializeField]
    SteamVR_Input_Sources handType = SteamVR_Input_Sources.RightHand;

    List<Color> defaultColors;
    private GrabbableObject w;
    private Hand hand;
    SkinnedMeshRenderer mr;
    bool isIncarnated;
    Barracks barrack;

    private bool shouldMove = false;

    public override SelectableTypes Type
    {
        get
        {
            return SelectableTypes.Ally;
        }
    }


    public void Incarnate()
    {

        PlayerStats player = Player.instance.GetComponent<PlayerStats>();
        player.Incarnate(this);

    }


    public override void SetPoint(Vector3 position)
    {
        if (!PriorityMoving)
        {
            base.SetPoint(position);
            //availability = true;
        }
    }

    public void SetPrioirityPoint(Vector3 position)
    {
        PriorityMoving = false;
        SetPoint(position);
        PriorityMoving = true;
        availability = false;
    }
    /* private void Start()
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
     */

    public override void OnSelected(Valve.VR.InteractionSystem.Hand hand)
    {
        base.OnSelected(hand);
        //ChangeSpecificColor(Color.green);
        var rm = GetMyRadialMenu;
        rm.top.AddFunctionOnPress(Incarnated);
        rm.left.AddFunctionOnPress(MoveTowards);
        //rm.right.AddFunctionOnPress(SetPrioirityPoint());
        //rm.

    }

    private void MoveTowards()
    {
        UnitSelector.instance.allyMove = this;
    }


    public override void Diselected()
    {
        base.Diselected();
        //ChangeSpecificColor(Color.yellow);
        var rm = GetMyRadialMenu;
        rm.top.RemoveFunctionsOnPress();
        rm.left.RemoveFunctionsOnPress();
    }

    public void ChangeBarracks(AllyController other)
    {
        barrack = other.barrack;
        ChangeTarget(other);
        if (other.barrack != null)
            other.barrack.Occupy(this);
        availability = false;
    }

    public void Incarnated()
    {
        var rm = GetMyRadialMenu;
        rm.right.RemoveFunctionsOnPress();
        Incarnate();
    }

    public void SetBarrack(Barracks b)
    {
        if (barrack != null)
            barrack.DesOccupy();

        b.Occupy(this);

        barrack = b;
    }
}
