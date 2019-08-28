using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barracks : MonoBehaviour
{
    [SerializeField]
    Transform standPoint;
    public Transform GetStandPoint()
    {
        return standPoint;
    }
}
