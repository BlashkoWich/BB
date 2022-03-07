using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ParticleSystemMirrorSync : MonoBehaviour
{
    private BB.Weapon.ThrowProjectile throwProjectile;

    public void EffectSwithcer(bool isActivate)
    {
        isActivate = !isActivate;
    }
}
