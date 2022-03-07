using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class SyncParticleSystemList : NetworkBehaviour
{
    public Dictionary<int, List<ParticleSystem>> ParticleSystems = new Dictionary<int, List<ParticleSystem>>();
        public void SwitcherProjectileForAll(int numberProjectail,  bool activatorEffect)
    {
        if(isServer)
        {
            ServerSwitcherProjectileForAll(numberProjectail,activatorEffect);
        }
        else
        {
            CmdSwitcherProjectileForAll(numberProjectail, activatorEffect);
        }
    }
    
    [Command(requiresAuthority = false)]
    private void CmdSwitcherProjectileForAll(int numberProjectail, bool activatorEffect)
    {
        ServerSwitcherProjectileForAll(numberProjectail, activatorEffect);
    }
    [Server]
    private void ServerSwitcherProjectileForAll(int numberProjectail, bool activatorEffect)
    {
        RpcSwitcherProjectileForAll(numberProjectail,  activatorEffect);
    }
    [ClientRpc]
    private void RpcSwitcherProjectileForAll(int numberProjectail, bool activatorEffect)
    {
        List<ParticleSystem> particleSystems = ParticleSystems[numberProjectail];
        SwitcherEffect(particleSystems, activatorEffect);
    }

    private void SwitcherEffect(List<ParticleSystem> effects, bool activatorEffect)
    {
        if(activatorEffect == true)
            PlayEffect(effects);
        else
            StopEffect(effects);
    }

    private void PlayEffect(List<ParticleSystem> effects)
    {
        foreach (var effect in effects)
        {
            effect.Play();
        }
    }
    private void StopEffect(List<ParticleSystem> effects)
    {
        foreach (var effect in effects)
        {
            effect.Stop();
        }
    }
}
