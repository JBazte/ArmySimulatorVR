using UnityEngine.AI;
using UnityEngine;

[RequireComponent(typeof(NavMeshAgent), typeof(CharacterStats))]
public class EnemyController : MonoBehaviour
{
    NavMeshAgent agent;
    [SerializeField]
    Transform movePoint;
    [SerializeField]
    LayerMask detectMask;
    [SerializeField]
    Transform shotSpawnPosition;
    [SerializeField]
    Shot shotInstance;


    [SerializeField]
    float detectRadious;
    [SerializeField]
    float attackRadious;
    [SerializeField]
    float shotForce;

    private CharacterStats stats;
    private EnemyController target;
    private float lastAttack;

    bool ischasingEnemy;
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        stats = GetComponent<CharacterStats>();
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

    }

    public void SetPoint(Transform position)
    {
        this.movePoint = position;
        agent.SetDestination(position.position);
        target = null;
        ischasingEnemy = false;
        agent.stoppingDistance = 0;
        agent.isStopped = false;

    }

    public void SetTarget(EnemyController target)
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
                    EnemyController enemy = enemies[index].GetComponentInParent<EnemyController>();
                    SetTarget(enemy);

                }
            }
        }
        else
        {
            if (movePoint != null)
                SetPoint(movePoint);
            ischasingEnemy = false;
        }
    }

    public void LookAtTarget()
    {
        transform.LookAt(target.transform);
    }

    public void ChaseTargets()
    {
        LookAtTarget();
        float distance = Vector3.Distance(transform.position, target.transform.position);
        if (distance <= attackRadious)
        {
            agent.isStopped = true;
            Shoot();
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
            /* CharacterStats targetStats = target.GetComponent<CharacterStats>();
            targetStats.TakeDamage(stats.Damage);  
            */

            // Replace with Pool Later
            lastAttack = 1 / stats.AttackSpeed;
            Shot instance = Instantiate(shotInstance, shotSpawnPosition.position, transform.rotation);
            instance.SetShot(stats.Damage, detectMask);
            Rigidbody rb = instance.GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * shotForce, ForceMode.Impulse);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, detectRadious);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadious);
    }


}
