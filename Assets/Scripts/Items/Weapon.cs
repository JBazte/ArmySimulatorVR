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
    private Transform startMagazineLinearDrivePosition;
    [SerializeField]
    private Transform endMagazineLinearDrivePosition;
    [SerializeField]
    private Transform magazinePostition;
    [SerializeField]
    private LayerMask magazineLayer;
    [SerializeField]
    private bool isLinearDrive = true;
    [SerializeField]
    private ParticleSystem muzzleFlash;


    [Header("Stats")]
    [SerializeField]
    private bool isAutomaic = true;
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

    public MagazineTypes MagazineType
    {
        get
        {
            return ammoType;
        }
    }
    [Header("Recoil")]
    [SerializeField]
    private float startingRecoilForce = 5f;
    [SerializeField]
    private float coolOffTime = 1f;
    [SerializeField]
    private LayerMask attackMask;
    [SerializeField]
    private float recoilForceMultiplier = 10f;

    protected float currentAmmo;
    private float lastAttack;
    private bool isReloading = false;
    private Rigidbody rb;
    private float recoilTime;
    private float shotCount;
    private float magazinetoNullTime = 3f;
    private float timeBeforeReload;
    protected bool hasMagazine;

    private bool isLinearDriving;
    protected Magazine lastMag;
    public float GetCurrentAmmo
    {
        get
        {
            return currentAmmo;
        }
    }

    private const float maxForce = 10f;
    private const float randomRecoil = 15f;
    private const float attachAngle = 30f;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (!hasMagazine)
        {
            currentAmmo = maxAmmo;
        }
    }
    private void Update()
    {
        lastAttack -= Time.deltaTime;
        timeBeforeReload -= Time.deltaTime;
        if (recoilTime > 0)
            recoilTime -= (Time.deltaTime * (recoilTime / coolOffTime));
        CheckAmunition();

    }
    public virtual void Shoot()
    {
        if (lastAttack < 0)
        {
            if (currentAmmo > 0)
            {
                float recoilForce = ApplyRecoil();
                lastAttack = 1 / attackSpeed;
                Shot instance = Instantiate(shotInstance);
                instance.transform.position = bulletSpawnPosition.position;
                Vector3 offset = Random.insideUnitCircle * recoilForce / maxForce / randomRecoil;
                instance.GetComponent<Rigidbody>().AddForce((offset + bulletSpawnPosition.forward) * bulletForce, ForceMode.Impulse);
                instance.SetShot(damage, attackMask);
                currentAmmo--;
                recoilTime++;
                if (muzzleFlash != null)
                    muzzleFlash.Play();
            }
            else
            {
                if (!isReloading)
                {
                    //StartCoroutine(Reload());
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
            // Debug.Log("attached");

            magazine.OnAttachedToWeapon(this);
            currentAmmo = magazine.GetCurrentAmmo;
            hasMagazine = true;
            magazine.transform.SetParent(transform);
            magazine.transform.localPosition = magazinePostition.localPosition;
            magazine.transform.localRotation = magazinePostition.localRotation;

        }
    }

    public void CheckAmunition()
    {
        if (!hasMagazine && !isLinearDriving)
        {
            Collider[] mags = Physics.OverlapSphere(magazinePostition.position, checkMagazineRadious, magazineLayer);

            foreach (var mag in mags)
            {
                float distance = Vector3.Distance(magazinePostition.position, mag.transform.position);
                Magazine magazine = mag.GetComponentInParent<Magazine>();
                if (!isLinearDriving)
                {
                    if (lastMag == magazine)
                    {
                        if (distance > startMagazineLinearDrive)
                        {
                            lastMag = null;
                        }
                    }
                }
                if (timeBeforeReload >= 0)
                {
                    lastMag = null;
                }
                if (distance < startMagazineLinearDrive)
                {

                    if (lastMag != magazine)
                    {

                        float angle = Quaternion.Angle(magazinePostition.rotation, magazine.transform.rotation);
                        Debug.Log(angle);
                        if (angle <= attachAngle)
                        {
                            lastMag = magazine;
                            if (magazine.GetComponent<Interactable>().attachedToHand == null || !isLinearDrive)
                            {
                                AttachMagazine(magazine);
                                return;
                            }

                            else
                            {

                                StartLinearDrive(magazine);


                                return;
                            }
                        }

                    }

                }
            }
        }
    }

    public void StartLinearDrive(Magazine magazine)
    {

        MyLinearDrive ld = magazine.GetComponent<MyLinearDrive>();
        if (ld != null)
        {

            isLinearDriving = true;
            //Physics.IgnoreCollision(GetComponentInChildren<Collider>(), magazine.GetComponentInChildren<Collider>(), true);
            ld.startPosition = startMagazineLinearDrivePosition;
            ld.endPosition = endMagazineLinearDrivePosition;
            ld.startingRotation = magazinePostition.rotation;
            ld.OnActivated(GetComponentInChildren<LinearMapping>(), this);
        }
        else
        {
            DisAttachMagazine();
        }

    }

    public void EndLinearDrive()
    {
        isLinearDriving = false;
    }

    public void DisAttachMagazine()
    {
        currentAmmo = 0;
        hasMagazine = false;
        timeBeforeReload = magazinetoNullTime;
        Debug.Log("disattached");
    }

    private void HandAttachedUpdate(Hand hand)
    {


        if (isAutomaic)
        {
            if (SteamVR_Input.GetState("Shoot", hand.handType))
            {
                Shoot();
            }
            if (Input.GetKey(KeyCode.K))
            {
                Shoot();
            }
        }
        else
        {
            if (SteamVR_Input.GetStateDown("Shoot", hand.handType))
            {
                Shoot();
            }
            if (Input.GetKeyDown(KeyCode.K))
            {
                Shoot();
            }
        }


    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(magazinePostition.position, checkMagazineRadious);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(magazinePostition.position, startMagazineLinearDrive);
    }
}
