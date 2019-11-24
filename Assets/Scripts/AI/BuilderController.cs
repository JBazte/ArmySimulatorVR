using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuilderController : MedicController
{
    [SerializeField]
    private GameObject[] buildingsPrefabs;
    [SerializeField]
    private float totalBuildingTime;


    private Vector3 constructPosition;
    private Quaternion constructionRotation;

    public bool isBuilding;
    private int buildingIndex;

    private Barracks objAssignedToConstruct;

    private const float buildTick = 10;
    private void Construct(int index, Vector3 position)
    {
        checkHealing = false;
        CharacterStats instance = Instantiate(buildingsPrefabs[index]).GetComponent<CharacterStats>();
        instance.transform.position = position;
        instance.transform.rotation = constructionRotation;
        isBuilding = false;
        agent.isStopped = true;

        StartCoroutine(Build(instance));


    }
    private void Construct(Barracks objToConstruct, Vector3 position)
    {
        CharacterStats instance = Instantiate(objToConstruct).GetComponent<CharacterStats>();
        instance.transform.position = position;
        instance.transform.rotation = constructionRotation;
        isBuilding = false;

        agent.isStopped = true;
        StartCoroutine(Build(instance));


    }


    private IEnumerator Build(CharacterStats stats)
    {

        stats.TakeDamage(stats.MaxHealth - 1);
        float buildingProgres = 0;
        controller.SetAvailability = false;
        float time = totalBuildingTime / buildTick;
        while (buildingProgres < 100)
        {
            yield return new WaitForSeconds(time);
            buildingProgres += buildTick;
            stats.HealDamage(stats.MaxHealth / buildTick);
            Debug.Log(buildingProgres);

        }
        ReturnHome();
        controller.SetAvailability = true;
    }

    public void SetConstruction(int index, Vector3 position)
    {
        agent.SetDestination(position);
        constructPosition = position;
        buildingIndex = index;
        isBuilding = true;

    }

    public void SetConstruction(Barracks objToConstruct, Transform t)
    {

        AllyController a = GetComponent<AllyController>();
        a.SetPrioirityPoint(t.position);
        constructPosition = t.position;
        constructionRotation = t.rotation;
        isBuilding = true;
        objAssignedToConstruct = objToConstruct;
        home = transform.position;


    }



    public void Checkdistance()
    {
        if (isBuilding)
        {
            float distance = Vector3.Distance(transform.position, constructPosition);
            if (distance <= interactRadious)
            {
                Construct(objAssignedToConstruct, constructPosition);
            }
        }
    }


    new private void Update()
    {
        base.Update();
        Checkdistance();
    }

}
