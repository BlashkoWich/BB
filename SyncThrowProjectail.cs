using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class SyncThrowProjectail : NetworkBehaviour
{
    public BB.Weapon.ThrowProjectile[] _throwProjectail;

    private void Start()
    {
        foreach (var projectile in _throwProjectail)
        {
            projectile.syncThrowProjectail = this;
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
        _throwProjectail[numberProjectail].Invoke(methodName, 0.01f);
    }
}
