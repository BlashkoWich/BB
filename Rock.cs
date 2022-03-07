using System.Collections;
using System.Collections.Generic;
using BB.Weapon;
using UnityEngine;
using Mirror;

public class Rock : NetworkBehaviour
{
    [SerializeField]
    private GameObject _mainObject;

    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<BB.Weapon.Projectile>(out var projectile))
        {
            projectile.OnDamageDelievered();
            if (projectile is BB.Weapon.UltimateRangeProjectiles)
            {
                Break();
            }
        }
    }

    private void Break()
    {
        if(isServer)
        {
            ServerBrake();
        }
        else
        {
            CmdBreak();
        }
    }

    [Command(requiresAuthority = false)]
    private void CmdBreak()
    {
        ServerBrake();
    }

    [Server]
    private void ServerBrake()
    {
        RpcBrake();
    }

    [ClientRpc]
    private void RpcBrake()
    {
        _mainObject.SetActive(false);
    }
}
