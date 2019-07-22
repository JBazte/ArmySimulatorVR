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
    private Weapon attachedWeapon;

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

    public void OnAttachedToWeapon(Weapon weapon)
    {
        col.isTrigger = true;
        rb.isKinematic = true;
        attachedWeapon = weapon;
    }

    public void OnDettachedFromWeapon()
    {
        col.isTrigger = false;
        rb.isKinematic = false;
        if (attachedWeapon != null)
        {
            currentAmmo = attachedWeapon.GetCurrentAmmo;
            attachedWeapon.DisAttachMagazine();
            Physics.IgnoreCollision(GetComponentInChildren<Collider>(), attachedWeapon.GetComponentInChildren<Collider>(), true);
        }
        transform.parent = null;
        attachedWeapon = null;
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