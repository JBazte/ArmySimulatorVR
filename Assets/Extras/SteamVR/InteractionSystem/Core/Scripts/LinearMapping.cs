//======= Copyright (c) Valve Corporation, All rights reserved. ===============
//
// Purpose: A linear mapping value that is used by other components
//
//=============================================================================

using UnityEngine;
using System.Collections;

namespace Valve.VR.InteractionSystem
{
    //-------------------------------------------------------------------------
    public class LinearMapping : MonoBehaviour
    {
        public float value;

        private float originalValue;

        void Start()
        {
            originalValue = value;
        }
        public void RestoreOriginalValue()
        {
            value = originalValue;
        }
        public void RestoreInvertedOriginalValue()
        {
            value = 1f - originalValue;
        }
    }
}
