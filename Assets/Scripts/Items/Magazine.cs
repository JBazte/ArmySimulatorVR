using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class Magazine : MonoBehaviour
{

    [SerializeField]
    private MagazineTypes type = MagazineTypes.Rifle;
    [SerializeField]
    private float maxAmmo = 30f;

    private float currentAmmo;
    private Collider col;
    private Rigidbody rb;
    public float GetCurrentAmmo
    {
        get
        {
            return currentAmmo;
        }
    }

    public MagazineTypes GetMagazineType
    {
        get
        {
            return type;
        }
    }

    void Start()
    {
        currentAmmo = maxAmmo;
        col = GetComponentInChildren<Collider>();
        rb = GetComponentInChildren<Rigidbody>();

        Weapon weapon = GetComponentInParent<Weapon>();
        if (weapon != null)
        {
            weapon.AttachMagazine(this);
        }
    }
    //-------------------------------------------------
    // Called every Update() while this GameObject is attached to the hand
    //-------------------------------------------------
    private void OnAttachedToHand(Hand hand)
    {
        OnDettachedFromWeapon();
    }

    public void OnAttachedToWeapon()
    {
        col.isTrigger = true;
        rb.isKinematic = true;
    }

    public void OnDettachedFromWeapon()
    {
        col.isTrigger = false;
        rb.isKinematic = false;
    }
    public void RemoveAmmo(float amount)
    {
        if (amount > 0)
        {
            currentAmmo -= amount;
        }
    }
    public void ChangeAmount(float amount)
    {
        if (amount > 0)
        {
            currentAmmo = amount;
        }
    }

}

public enum MagazineTypes
{
    Pistol,
    Rifle
}