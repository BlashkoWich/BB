using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BB.ScriptableObjects
{
    [CreateAssetMenu(fileName = "ManagerSettings", menuName = "BrawlBunch/ManagerSettings", order = 0)]
    public class ManagerSettings : ScriptableObject
    {
        [Range(0, 100)]
        public float MinimalPercentageToDrawAim;
    }
}