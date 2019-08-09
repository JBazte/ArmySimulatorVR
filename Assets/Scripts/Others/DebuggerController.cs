using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebuggerController : MonoBehaviour
{
    public AllyController[] alliesToEncarnate;
    AllyController previosIncarnate;
    private KeyCode[] numbers = {
        KeyCode.Alpha0,
        KeyCode.Alpha1,
        KeyCode.Alpha2,
        KeyCode.Alpha3,
        KeyCode.Alpha4,
        KeyCode.Alpha5,
        KeyCode.Alpha6,
        KeyCode.Alpha7,
        KeyCode.Alpha8,
        KeyCode.Alpha9
    };

    private void Update()
    {
        for (int i = 0; i < alliesToEncarnate.Length; i++)
        {

            if (Input.GetKeyDown(numbers[i]))
            {
                if (previosIncarnate != null)
                {
                    previosIncarnate.DisIncarnate();
                }
                previosIncarnate = alliesToEncarnate[i];
                previosIncarnate.Incarnate();
            }

        }
    }
}

