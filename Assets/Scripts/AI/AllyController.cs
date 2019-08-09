using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;
using Valve.VR;
public class AllyController : EnemyController
{
    [SerializeField]
    GrabableObject objectPrefab;
    [SerializeField]
    SteamVR_Input_Sources handType = SteamVR_Input_Sources.RightHand;


    private GrabableObject w;
    private Hand hand;
    public void Incarnate()
    {
        w = Instantiate(objectPrefab);

        Player player = Player.instance;
        Hand hand = player.GetHand(handType);
        if (hand == null)
        {
            hand = player.GetHand(SteamVR_Input_Sources.Any);
        }
        this.hand = hand;
        w.AttachToHand(hand);

        player.transform.position = transform.position;

        gameObject.SetActive(false);
    }

    public void DisIncarnate()
    {
        gameObject.SetActive(true);
        if (w != null)
        {
            hand.DetachObject(w.gameObject, false);
            Destroy(w.gameObject);
        }
    }
}
