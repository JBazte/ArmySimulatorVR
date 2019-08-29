using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagazineSpawner : ItemSpawner
{
    [SerializeField]
    private Magazine[] magazines;
    private MagazineTypes currentType;
    public void ChangeType(MagazineTypes type){
        currentType = type;
        item = magazines[(int)type].GetComponent<GrabableObject>();
    }
}
