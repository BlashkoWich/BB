using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class SyncUltimateRangeProjectail : NetworkBehaviour
{
    public BB.Weapon.UltimateRangeProjectiles[] _ultimateRangeProjectail;

    private void Start()
    {
        foreach (var projectile in _ultimateRangeProjectail)
        {
            projectile.syncUltimateRangeProjectail = this;
        }
    }

    public void ActivateMethod(int numberProjectail, string methodName)
    {
        if(isServer)
        {
            ServerActivateMethod(numberProjectail, methodName);
        }
        else
        {
            CmdActivateMethod(numberProjectail, methodName);
        }
    }

    [Command(requiresAuthority = false)]
    private void CmdActivateMethod(int numberProjectail, string methodName)
    {
        ServerActivateMethod(numberProjectail, methodName);
    }
    [Server]
    private void ServerActivateMethod(int numberProjectail, string methodName)
    {
        RpcActivateMethod(numberProjectail, methodName);
    }
    [ClientRpc]
    private void RpcActivateMethod(int numberProjectail, string methodName)
    {
        _ultimateRangeProjectail[numberProjectail].Invoke(methodName, 0.01f);
    }
}
