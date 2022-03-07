using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BB.Weapon
{
    public interface IWeaponActionObservable
    {
        void AddObserver(IWeaponActionObserver observer);
        void RemoveObserver(IWeaponActionObserver observer);
        void NotifyObservers(WeaponActionInfo weaponActionInfo);
    }
}
