using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class SyncGameobjectActivate : NetworkBehaviour
{
    [SerializeField]
    private GameObject[] _projectail;
    public void SwitcherProjectileForAll(int numberProjectail, bool activatorProjectail)
    {
        if(isServer)
        {
            ServerSwitcherProjectileForAll(numberProjectail, activatorProjectail);
        }
        else
        {
            CmdSwitcherProjectileForAll(numberProjectail, activatorProjectail);
        }
    }
    
    [Command(requiresAuthority = false)]
    private void CmdSwitcherProjectileForAll(int numberProjectail, bool activatorProjectail)
    {
        ServerSwitcherProjectileForAll(numberProjectail, activatorProjectail);
    }
    [Server]
    private void ServerSwitcherProjectileForAll(int numberProjectail,bool activatorProjectail)
    {
        RpcSwitcherProjectileForAll(numberProjectail, activatorProjectail);
    }
    [ClientRpc]
    private void RpcSwitcherProjectileForAll(int numberProjectail,bool activatorProjectail)
    {
        _projectail[numberProjectail].SetActive(activatorProjectail);
    }

    private void SwitcherEffect(ParticleSystem effect, bool activatorEffect)
    {
        if(activatorEffect == true)
            PlayEffect(effect);
        else
            StopEffect(effect);
    }

    private void PlayEffect(ParticleSystem effect)
    {
        effect.Play();
    }
    private void StopEffect(ParticleSystem effect)
    {
        effect.Stop();
    }
}

