using UnityEngine.AI;
using UnityEngine;

[RequireComponent(typeof(NavMeshAgent), typeof(CharacterStatas))]
public class EnemyController : MonoBehaviour
{
    NavMeshAgent agent;
    [SerializeField]
    Transform target;
    [SerializeField]
    LayerMask detectMask;

    [SerializeField]
    float detectRadious;
    [SerializeField]
    float attackRadious;

    CharacterStatas stats;
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        stats = GetComponent<CharacterStatas>();
        SetTarget(target);
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
        agent.SetDestination(target.position);
    }

    private void Update()
    {

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
                    EnemyController target = enemies[index].GetComponent<EnemyController>();

                }
            }
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
