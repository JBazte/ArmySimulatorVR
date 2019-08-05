using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;
using Valve.VR;
public class AllyController : EnemyController
{
    [SerializeField]
    private Weapon weaponPrefab;
    [SerializeField]
    SteamVR_Input_Sources handType = SteamVR_Input_Sources.RightHand;


    public void Incarnate()
    {
        Weapon w = Instantiate(weaponPrefab);
        Hand hand = null;

        if (handType == SteamVR_Input_Sources.RightHand)
        {
            hand = PlayerController.instance.GetComponent<Player>().rightHand;
        }
        else if (handType == SteamVR_Input_Sources.LeftHand)
        {
            hand = PlayerController.instance.GetComponent<Player>().leftHand;
        }

        if (hand == null)
        {
            // hand = PlayerController.instance.GetComponent<Player>().GetHand;
        }

        this.enabled = false;
    }
}
