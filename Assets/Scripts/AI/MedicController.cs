
using UnityEngine;
using UnityEngine.AI;

public class MedicController : Selectable
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

    AllyController controller;
    protected NavMeshAgent agent;
    private float lastHeal;

    public override SelectableTypes Type
    {
        get
        {
            return SelectableTypes.Medic;
        }
    }

    protected void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (target != null)
        {
            SetTarget(target);
        }
        agent.stoppingDistance = interactRadious;
        controller = GetComponent<AllyController>();
    }
    public void SetTarget(CharacterStats target)
    {

        if (healingMask == (healingMask | (1 << target.gameObject.layer)))
        {
            agent.SetDestination(target.transform.position);
            controller.SetPrioirityPoint(target.transform.position);
            this.target = target;
            controller.priorityMoving = true;

        }
    }

    protected void Update()
    {
        lastHeal -= Time.deltaTime;
        if (target != null)
        {
            Heal();
        }
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
                        controller.priorityMoving = false;
                    }
                }
            }
        }
    }

    protected void ReturnHome()
    {
        agent.isStopped = false;
        if (home == null)
        {
            controller.ReturnHome();
        }
        else
        {
            agent.SetDestination(home.transform.position);
        }

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, interactRadious);
    }

    public override void OnSelected(Valve.VR.InteractionSystem.Hand hand)
    {
        ShowRadialMenu();
    }

    public override void Diselected()
    {
        HideRadialMenu();
    }
    public override void AfterSelected(Selectable selectable)
    {
        controller.priorityMoving = false;
        if (selectable.Type == SelectableTypes.Ally)
        {
            AllyController ally = selectable as AllyController;
            SetTarget(ally.GetComponent<CharacterStats>());

        }
        else
        {
            controller.AfterSelected(selectable);
        }
    }
}
