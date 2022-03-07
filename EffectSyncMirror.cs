using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class EffectSyncMirror : NetworkBehaviour
{
    private BB.Weapon.ThrowProjectile _projectail;
    [SerializeField]
    private GameObject _throwProjectile;
    private GameObject _puddle;
    private ParticleSystem _throwWrapper;
    private bool isActivate = false;
    private bool isActivatePuddle = false;
    private bool isActivateThrowWrapper = false;

    private bool throwWapperIsPlay = false;

    public void EffectSwitcher(bool mainActivator)
    {
        if(isServer)
        {
            ActivateEffectForAll(mainActivator);
        }
        else
        {
            CmdActivateEffectForAll(mainActivator);
        }
    }

    public void SetEffects(GameObject puddle, ParticleSystem throwWapper)
    {
        _puddle = puddle;
        _throwWrapper = throwWapper;
    }
    [Command(requiresAuthority = false)]
    private void CmdActivateEffectForAll(bool activator)
    {
        ActivateEffectForAll(activator);
    }
    [Server]
    private void ActivateEffectForAll(bool activator)
    {
        isActivate = activator;
        isActivateThrowWrapper = activator;
        RpcActivateEffectForAll(activator);
    }

    [ClientRpc]
    private void RpcActivateEffectForAll(bool activator)
    {
        isActivate = activator;
        isActivateThrowWrapper = true;
    }
    public void PuddleSwitcher(bool activator)
    {
        isActivatePuddle = activator;
        if(isServer)
        {
            ActivatePuddleForAll(activator);
        }
        else
        {
            CmdActivatePuddleForAll(activator);
        }
    }
    [Command(requiresAuthority = false)]
    private void CmdActivatePuddleForAll(bool activator)
    {
        ActivatePuddleForAll(activator);
    }
    [Server]
    private void ActivatePuddleForAll(bool activator)
    {
        RpcActivatePuddleForAll(activator);
    }

    [ClientRpc]
    private void RpcActivatePuddleForAll(bool activator)
    {
        isActivatePuddle = activator;
        isActivateThrowWrapper = false;
    }

    private void Update()
    {
        if(_throwProjectile != null && _puddle != null && _throwWrapper != null)
        {
            if(isActivate == true)
            {
                Debug.Log(isActivateThrowWrapper + "ВЫФВФЫВФЫВФЫВФЫВФЫВ");
                _throwProjectile.SetActive(true);
                
                if(isActivatePuddle == true)
                {
                    _puddle.SetActive(true);
                }
                if(_throwWrapper.isPlaying == false && isActivateThrowWrapper == true)
                {
                    _throwWrapper.Play();
                    
                }
                if(isActivateThrowWrapper == false)
                {
                    _throwWrapper.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                }
            }
            else
            {
                _throwProjectile.SetActive(false);
                _puddle.SetActive(false);
                _throwWrapper.Stop();
            }   
        }
    }

    private void Start()
    {
        
    }
}
