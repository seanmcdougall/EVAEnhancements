using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace EVAEnhancements
{
    [KSPAddon(KSPAddon.Startup.Flight, false)]
    public class EVAEnhancementsBehaviour : MonoBehaviour
    {
        GameObject navBall = null;
        NavBall ball = null;

        internal void Update()
        {
            if (navBall == null)
            {
                // Get a pointer to the navball
                navBall = GameObject.Find("NavBall");
                ball = navBall.GetComponent<NavBall>();
            }
        }

    }
}
