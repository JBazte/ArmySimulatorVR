using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;
using Valve.VR;


public class Weapon : MonoBehaviour
{
    [SerializeField]
    private Shot shotInstance;
    [SerializeField]
    private Transform bulletSpawnPosition;
    [SerializeField]
    private float bulletForce = 10f;
    [Header("Stats")]
    [SerializeField]
    private float damage = 1f;
    [SerializeField]
    private float maxAmmo = 30f;
    [SerializeField]
    private float totalReloadTime = 3f;
    [SerializeField]
    private float attackSpeed = 1f;
    [SerializeField]
    private float startingRecoilForce = 5f;
    [SerializeField]
    private float coolOffTime = 1f;
    [SerializeField]
    LayerMask attackMask;


    private float currentAmmo;
    private float lastAttack;
    private bool isReloading = false;
    private Rigidbody rb;
    private float recoilTime;
    private void Start()
    {
        currentAmmo = maxAmmo;
        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        lastAttack -= Time.deltaTime;
    }
    public void Shoot()
    {
        if (lastAttack < 0)
        {
            if (currentAmmo > 0)
            {
                ApplyRecoil();
                lastAttack = 1 / attackSpeed;
                Shot instance = Instantiate(shotInstance);
                instance.transform.position = bulletSpawnPosition.position;
                instance.GetComponent<Rigidbody>().AddForce(bulletSpawnPosition.forward * bulletForce, ForceMode.Impulse);
                instance.SetShot(damage, attackMask);
                currentAmmo--;
            }
            else
            {
                if (!isReloading)
                {
                    StartCoroutine(Reload());
                }
            }
        }
    }

    private IEnumerator Reload()
    {
        isReloading = true;
        yield return new WaitForSeconds(totalReloadTime);
        currentAmmo = maxAmmo;
        isReloading = false;
    }

    private void ApplyRecoil()
    {
        float recoilForce;
        rb.AddForceAtPosition((bulletSpawnPosition.up * recoilForce) + (bulletSpawnPosition.forward * recoilForce), bulletSpawnPosition.position, ForceMode.Impulse);
    }

    private void HandAttachedUpdate(Hand hand)
    {

        if (Input.GetKey(KeyCode.K))
        {
            Shoot();
        }
        /* if (SteamVR_Input.GetStateDown("Shoot", SteamVR_Input_Sources.RightHand))
         {
             Shoot();
         }
         */

    }
}
