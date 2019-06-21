
using UnityEngine;
using UnityEngine.AI;

public class MedicController : MonoBehaviour
{

    [SerializeField]
    Transform home;
    [SerializeField]
    private EnemyController target;
    [SerializeField]
    float interactRadious;
    [SerializeField]
    private float healing;
    [SerializeField]
    private float timePerHeal;
    [SerializeField]
    private float healDelay;
    [SerializeField]
    private LayerMask healingMask;


    private NavMeshAgent agent;


    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (target != null)
        {
            SetTarget(target);
        }
        agent.stoppingDistance = interactRadious;
    }
    public void SetTarget(EnemyController target)
    {

        if (healingMask == (healingMask | (1 << target.gameObject.layer)))
            agent.SetDestination(target.transform.position);
    }

    private void Update()
    {
        timePerHeal -= Time.deltaTime;
        Heal();
    }

    private void Heal()
    {
        float distance = Vector3.Distance(transform.position, target.transform.position);
        if (distance <= interactRadious)
        {
            if (timePerHeal < 0)
            {
                timePerHeal = 1 / healDelay;
                CharacterStats targetStats = target.GetComponent<CharacterStats>();
                if (targetStats != null)
                {
                    bool hasFinished = targetStats.HealDamage(healing);
                    if (hasFinished)
                    {
                        ReturnHome();
                    }
                }
            }
        }
    }

    private void ReturnHome()
    {
        agent.SetDestination(home.transform.position);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, interactRadious);
    }

}
