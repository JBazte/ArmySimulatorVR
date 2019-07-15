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

    [Header("Recoil")]
    [SerializeField]
    private float startingRecoilForce = 5f;
    [SerializeField]
    private float coolOffTime = 1f;
    [SerializeField]
    private LayerMask attackMask;
    [SerializeField]
    private float recoilForceDivider = 10f;


    private float currentAmmo;
    private float lastAttack;
    private bool isReloading = false;
    private Rigidbody rb;
    private float recoilTime;
    private float shotCount;

    private float maxForce = 10f;

    private void Start()
    {
        currentAmmo = maxAmmo;
        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        lastAttack -= Time.deltaTime;
        if (recoilTime > 0)
            recoilTime -= (Time.deltaTime * (1 / coolOffTime)) * (recoilTime + 0.1f);

    }
    public void Shoot()
    {
        if (lastAttack < 0)
        {
            if (currentAmmo > 0)
            {
                float recoilForce = ApplyRecoil();
                lastAttack = 1 / attackSpeed;
                Shot instance = Instantiate(shotInstance);
                instance.transform.position = bulletSpawnPosition.position;
                Vector3 offset = Random.insideUnitCircle * recoilForce / maxForce / 10;
                instance.GetComponent<Rigidbody>().AddForce((offset + bulletSpawnPosition.forward) * bulletForce, ForceMode.Impulse);
                instance.SetShot(damage, attackMask);
                currentAmmo--;
                recoilTime++;
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

    private float ApplyRecoil()
    {
        float recoilForce = startingRecoilForce;
        float r = CuadraticFunction(recoilForce);
        if (r < 0)
        {
            r = 0;
        }
        if (recoilForce > maxForce)
        {
            recoilForce = maxForce;
        }
        recoilForce += startingRecoilForce;
        //recoilForce 
        Debug.Log(recoilForce);
        rb.AddForceAtPosition((bulletSpawnPosition.up * recoilForce) + (bulletSpawnPosition.forward * recoilForce), bulletSpawnPosition.position, ForceMode.Impulse);
        return recoilForce;
    }

    private float CuadraticFunction(float x)
    {
        return x * x * (1 / recoilForceDivider);

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
