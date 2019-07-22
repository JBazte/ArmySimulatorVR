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
    [SerializeField]
    private float checkMagazineRadious = 3f;
    [SerializeField]
    private float startMagazineLinearDrive = .5f;
    [SerializeField]
    private LayerMask magazineLayer;
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
    private MagazineTypes ammoType = MagazineTypes.Rifle;
    [SerializeField]
    private Transform magazinePostition;

    [Header("Recoil")]
    [SerializeField]
    private float startingRecoilForce = 5f;
    [SerializeField]
    private float coolOffTime = 1f;
    [SerializeField]
    private LayerMask attackMask;
    [SerializeField]
    private float recoilForceMultiplier = 10f;

    private float currentAmmo;
    private float lastAttack;
    private bool isReloading = false;
    private Rigidbody rb;
    private float recoilTime;
    private float shotCount;
    private bool hasMagazine;
    private MyLinearDrive linearDrive;

    public float GetCurrentAmmo
    {
        get
        {
            return currentAmmo;
        }
    }

    private const float maxForce = 10f;

    private void Start()
    {
        currentAmmo = maxAmmo;
        rb = GetComponent<Rigidbody>();
        linearDrive = GetComponentInChildren<MyLinearDrive>();
    }
    private void Update()
    {
        lastAttack -= Time.deltaTime;
        if (recoilTime > 0)
            recoilTime -= (Time.deltaTime * (recoilTime / coolOffTime));
        CheckAmunition();

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

        float recoilForce = CuadraticFunction(recoilTime) / (attackMask / 4);
        if (recoilForce < 0)
        {
            recoilForce = 0;
        }
        if (recoilForce > maxForce)
        {
            recoilForce = maxForce;
        }
        //recoilForce 
        //Debug.Log(recoilForce);
        rb.AddForceAtPosition((bulletSpawnPosition.up * recoilForce) + (bulletSpawnPosition.forward * recoilForce), bulletSpawnPosition.position, ForceMode.Impulse);
        return recoilForce;
    }

    private float CuadraticFunction(float x)
    {
        return x * x * (1 * recoilForceMultiplier) + startingRecoilForce;

    }

    public void AttachMagazine(Magazine magazine)
    {
        if (magazine.GetMagazineType == ammoType)
        {
            currentAmmo = magazine.GetCurrentAmmo;
            hasMagazine = true;
            magazine.transform.SetParent(transform);
            magazine.transform.position = magazinePostition.position;
            magazine.OnAttachedToWeapon(this);
        }
    }

    public void CheckAmunition()
    {
        Collider[] mags = Physics.OverlapSphere(magazinePostition.position, checkMagazineRadious, magazineLayer);

        foreach (var mag in mags)
        {
            if (Vector3.Distance(transform.position, mag.transform.position) > startMagazineLinearDrive)
            {
                StartLinearDrive();
            }
        }
    }

    public void StartLinearDrive()
    {

    }
    public void DisAttachMagazine()
    {
        currentAmmo = 0;
        hasMagazine = false;
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
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(magazinePostition.position, checkMagazineRadious);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(magazinePostition.position, startMagazineLinearDrive);
    }
}
