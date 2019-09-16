using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sniper : Weapon
{

    public override void Shoot()
    {
       
        base.Shoot();

        if (lastMag != null)
            Destroy(lastMag.gameObject);
        lastMag = null;
        hasMagazine = false;
    }
}
