using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BB.Weapon
{
    public interface IWeaponActionObserver
    {
        void Update(WeaponActionInfo weaponActionInfo);
    }
}
