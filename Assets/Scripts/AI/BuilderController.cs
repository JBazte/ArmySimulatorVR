using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuilderController : MedicController
{
    [SerializeField]
    private GameObject[] buildingsPrefabs;
    [SerializeField]
    private float totalBuildingTime;

    [SerializeField]
    private Transform constructPosition;
    private bool isBuilding;
    private int buildingIndex;

    private const float buildTick = 10;
    public void Construct(int index, Vector3 position)
    {
        CharacterStats instance = Instantiate(buildingsPrefabs[index]).GetComponent<CharacterStats>();
        instance.transform.position += position;
        isBuilding = false;
        agent.isStopped = true;
        StartCoroutine(Build(instance));


    }

    private IEnumerator Build(CharacterStats stats)
    {
        stats.TakeDamage(stats.MaxHealth - 1);
        float buildingProgres = 0;
        float time = totalBuildingTime / buildTick;
        while (buildingProgres < 100)
        {
            yield return new WaitForSeconds(time);
            buildingProgres += buildTick;
            stats.HealDamage(stats.MaxHealth / buildTick);
        }
        ReturnHome();
    }

    public void SetConstruction(int index, Transform position)
    {
        agent.SetDestination(position.position);
        constructPosition = position;
        buildingIndex = index;
        isBuilding = true;
    }

    private void Start()
    {
        base.Start();

    }

    public void Checkdistance()
    {
        if (isBuilding)
        {
            float distance = Vector3.Distance(transform.position, constructPosition.position);
            if (distance <= interactRadious)
            {
                Construct(buildingIndex, constructPosition.position);
            }
        }
    }

    new private void Update()
    {
        base.Update();
        Checkdistance();
    }
    public override void AfterSelected(Selectable selectable)
    {
        if (selectable.Type == SelectableTypes.Barracks)
        {
            Barracks bar = selectable as Barracks;
            SetTarget(bar.GetComponent<CharacterStats>());

        }
        else
        {
            base.AfterSelected(selectable);
        }

    }
}
