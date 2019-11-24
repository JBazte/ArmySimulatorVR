using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;



public class Tower : Barracks
{


    NavMeshAgent agent;


    bool hasEntered;
    private void Update()
    {

        if (occupant != null)
        {
            if (!hasEntered)
            {

                float distance = Vector3.Distance(occupant.transform.position, transform.position);
                if (distance <= 4f)
                {

                    occupant.transform.position = standPoint.position;
                    //occupant.GetComponent<NavMeshAgent>().Warp(standPoint.position);
                    //hasEntered = true;
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 4f);
    }
}
