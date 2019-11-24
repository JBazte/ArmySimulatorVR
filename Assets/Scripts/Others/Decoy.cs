using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Decoy : MonoBehaviour
{
    [SerializeField]
    Barracks realObject = null;


    public bool CallConstruct()
    {
        var possiblities = GameController.instance.GetEnemiesByType(EnemyTypes.Mechanic);
        possiblities.Sort(ClosestDistanceSort);
        bool hasFoundOne = false;
        if (possiblities.Count > 0)
        {
            foreach (AllyController ally in possiblities)
            {
                if (ally.GetAvailability)
                {
                    BuilderController b = ally.GetComponent<BuilderController>();

                    if (b != null)
                    {
                        b.SetConstruction(realObject, transform);
                        hasFoundOne = true;
                        break;

                    }


                }
            }
            // if (!hasFoundOne)
            //  possiblities[0].GetComponent<BuilderController>().SetConstruction(realObject, transform.position);

        }
        Destroy(gameObject);
        return hasFoundOne;
    }

    private int ClosestDistanceSort(AllyController x, AllyController y)
    {
        var distancex = Vector3.Distance(x.transform.position, transform.position);
        var distancey = Vector3.Distance(y.transform.position, transform.position);
        if (distancex > distancey)
        {
            return 1;
        }
        else
        {
            return -1;
        }

    }
}
