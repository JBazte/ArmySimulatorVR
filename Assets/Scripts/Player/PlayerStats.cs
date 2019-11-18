using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;
using Valve.VR;

public class PlayerStats : CharacterStats
{
    Player player;
    [SerializeField]
    SteamVR_Input_Sources handType = SteamVR_Input_Sources.RightHand;
    Hand hand;
    GrabbableObject objectInstance;
    private bool isIncarnated;

    CharacterStats incarnatedAlly;
    Belt belt;
    protected override void Die()
    {
        if (isIncarnated)
        {
            DisIncarnate();
        }
    }

    private void Start()
    {
        player = GetComponent<Player>();
        belt = GetComponentInChildren<Belt>();
        belt.gameObject.SetActive(false);
    }

    public void Incarnate(AllyController ally)
    {
        Debug.Log("Incarnated");
        UnitSelector.instance.BlockSelection = true;


        Hand hand = player.GetHand(handType);
        if (hand == null)
        {
            hand = player.GetHand(SteamVR_Input_Sources.Any);
        }
        this.hand = hand;
        objectInstance = Instantiate(ally.configuration.leftObject);
        objectInstance.transform.position = hand.transform.position;

        objectInstance.AttachToHand(hand);

        transform.position = ally.transform.position;
        transform.rotation = ally.transform.rotation;
        belt.ConfigureBelt(ally.configuration);
        belt.gameObject.SetActive(true);
        ally.gameObject.SetActive(false);

        CharacterStats cs = ally.GetComponent<CharacterStats>();
        incarnatedAlly = cs;
        currentHealth = cs.CurrentHealth;
        isIncarnated = true;


    }

    public void DisIncarnate()
    {
        belt.DeleteSlots();
        if (objectInstance != null)
        {
            hand.DetachObject(objectInstance.gameObject, false);
            hand.otherHand.DetachObject(hand.currentAttachedObject, false);
            Destroy(objectInstance.gameObject);
        }
        isIncarnated = false;
        incarnatedAlly.TakeDamage(incarnatedAlly.CurrentHealth - Mathf.Clamp(incarnatedAlly.CurrentHealth - currentHealth, 0, incarnatedAlly.MaxHealth));

        var selector = UnitSelector.instance;
        selector.BlockSelection = false;
        selector.ResetPlayerPositon();
        incarnatedAlly.gameObject.SetActive(true);
        belt.gameObject.SetActive(false);

    }

    private void Update()
    {
        if (isIncarnated)
        {
            if (SteamVR_Input.GetStateDown("Select", SteamVR_Input_Sources.Any))
            {
                DisIncarnate();

            }
        }
    }
}
