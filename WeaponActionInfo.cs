using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BB.Weapon
{
    public class WeaponActionInfo
    {
        public WeaponAction CurrentWeaponAction;
    }

    public enum WeaponAction
    {
        Shoot,
        StopShooting
    }
}