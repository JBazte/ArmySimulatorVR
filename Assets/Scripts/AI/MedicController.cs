
using UnityEngine;
using UnityEngine.AI;

public class MedicController : MonoBehaviour
{

    [SerializeField]
    Transform home;
    [SerializeField]
    private CharacterStats target;
    [SerializeField]
    protected float interactRadious;
    [SerializeField]
    private float healing;
    [SerializeField]
    private float healDelay;
    [SerializeField]
    private LayerMask healingMask;


    protected NavMeshAgent agent;
    private float lastHeal;

    protected void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (target != null)
        {
            SetTarget(target);
        }
        agent.stoppingDistance = interactRadious;
    }
    public void SetTarget(CharacterStats target)
    {

        if (healingMask == (healingMask | (1 << target.gameObject.layer)))
            agent.SetDestination(target.transform.position);
    }

    protected void Update()
    {
        lastHeal -= Time.deltaTime;
        Heal();
    }

    private void Heal()
    {
        float distance = Vector3.Distance(transform.position, target.transform.position);
        if (distance <= interactRadious + 0.1f)
        {
            if (lastHeal < 0)
            {
                lastHeal = 1 / healDelay;
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

    protected void ReturnHome()
    {
        agent.SetDestination(home.transform.position);
        agent.isStopped = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, interactRadious);
    }

}
