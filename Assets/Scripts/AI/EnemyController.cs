using UnityEngine.AI;
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent), typeof(CharacterStats))]
public class EnemyController : Enemy
{

    [SerializeField]
    private Vector3 movePoint;
    [SerializeField]
    private LayerMask detectMask;
    [SerializeField]
    private LayerMask attackMask;
    [SerializeField]
    private Transform shotSpawnPosition;
    [SerializeField]
    private Shot shotInstance;


    [SerializeField]
    private float detectRadious;
    [SerializeField]
    private float attackRadious;
    [SerializeField]
    private float shotForce;


    const float shotAccuaracy = 3;

    private NavMeshAgent agent;
    private CharacterStats stats;
    private CharacterStats target;
    private float lastAttack;
    private int currentAmmo;
    private bool isReloading;


    bool ischasingEnemy;
    private void Start()
    {


        if (movePoint != null)
        {
            SetPoint(movePoint);
        }
        else
        {
            agent.isStopped = true;
        }
        agent.speed = stats.Speed;
        agent.stoppingDistance = attackRadious;
        currentAmmo = stats.MaxAmmo;

    }
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        stats = GetComponent<CharacterStats>();
    }

    public void SetPoint(Transform position)
    {

        SetPoint(transform.position);

    }
    public void SetPoint(Vector3 position)
    {
        this.movePoint = position;
        agent.SetDestination(position);
        target = null;
        ischasingEnemy = false;
        agent.stoppingDistance = 0;
        agent.isStopped = false;
    }

    public void SetTarget(CharacterStats target)
    {
        this.target = target;
        agent.stoppingDistance = attackRadious;
        agent.SetDestination(target.transform.position);
        ischasingEnemy = true;
    }
    private void Update()
    {
        lastAttack -= Time.deltaTime;
        SearchTargets();
        if (ischasingEnemy)
            ChaseTargets();

        if (Input.GetKeyDown(KeyCode.T))
        {
            stats.TakeDamage(10f);
        }

    }

    private void SearchTargets()
    {
        if (Physics.CheckSphere(transform.position, detectRadious, detectMask))
        {
            Collider[] enemies = Physics.OverlapSphere(transform.position, detectRadious, detectMask);
            if (enemies.Length != 0)
            {
                float closestDistance = int.MaxValue;
                int index = 0;
                for (int i = 0; i < enemies.Length; i++)
                {
                    float distance = Vector3.Distance(transform.position, enemies[i].transform.position);
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        index = i;
                    }
                }
                if (closestDistance != int.MaxValue)
                {
                    CharacterStats enemy = enemies[index].GetComponentInParent<CharacterStats>();
                    SetTarget(enemy);

                }
            }
        }
        else
        {

            SetPoint(movePoint);
            ischasingEnemy = false;
        }
    }

    public void LookAtTarget()
    {
        Quaternion rotation = transform.rotation;
        transform.LookAt(target.transform);
        transform.rotation = new Quaternion(rotation.x, transform.rotation.y, transform.rotation.z, transform.rotation.w);
    }

    public void ChaseTargets()
    {
        LookAtTarget();

        float distance = Vector3.Distance(transform.position, target.transform.position);
        if (distance <= attackRadious)
        {
            RaycastHit hit;
            if (Physics.SphereCast(shotSpawnPosition.position, 2f, shotSpawnPosition.forward, out hit, 100f))
            {
                Shoot();
                if (attackMask == (attackMask | (1 << hit.transform.gameObject.layer)))
                {
                    agent.isStopped = true;

                }
                else
                {
                    agent.isStopped = false;

                }
            }

        }
        else
        {
            agent.isStopped = false;
        }
    }
    private void Shoot()
    {
        if (lastAttack <= 0)
        {
            if (currentAmmo > 0)
            {
                // Replace with Pool Later
                currentAmmo--;
                lastAttack = 1 / stats.AttackSpeed;
                Shot instance = Instantiate(shotInstance, shotSpawnPosition.position, transform.rotation);
                instance.SetShot(stats.Damage, attackMask);
                Vector3 offset = Random.insideUnitSphere * ((100 - stats.Accuracy) / 100) / shotAccuaracy;
                Rigidbody rb = instance.GetComponent<Rigidbody>();
                rb.AddForce((transform.forward + offset) * shotForce, ForceMode.Impulse);
            }
            else
            {
                if (!isReloading)
                    StartCoroutine(Reload());
            }
        }
    }

    private IEnumerator Reload()
    {
        isReloading = true;
        yield return new WaitForSeconds(stats.ReloadTime);
        currentAmmo = stats.MaxAmmo;
        isReloading = false;
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, detectRadious);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadious);
    }

    public override void Save(GameDataWriter writer)
    {
        base.Save(writer);
        writer.Write(stats.CurrentHealth);
        writer.Write(currentAmmo);
        writer.Write(lastAttack);
        writer.Write(movePoint);
    }

    public override void Load(GameDataReader reader)
    {
        base.Load(reader);
        stats.HealDamage(reader.ReadFloat() - stats.CurrentHealth);
        currentAmmo = reader.ReadInt();
        lastAttack = reader.ReadFloat();
        Vector3 position = reader.ReadVector3();
        SetPoint(position);
    }

}
