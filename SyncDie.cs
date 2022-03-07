using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using BB.Core;

[RequireComponent(typeof(SyncHealth))]
public class SyncDie : NetworkBehaviour
{
    private Character _character;

    private int _currentHealth => _syncHealth.CurrentHealth;

    [SerializeField]
    private GameObject[] _disableUponDeath;

    public GameObject[] DisableUponDeath => _disableUponDeath;

    private SyncHealth _syncHealth;
    private SyncRevive _syncRevive;

    private bool _isActivated = false;

    private bool isDie;

    void OnDisable()
    {
    }


    private void Start()
    {
        _character = GetComponent<BB.Core.Character>();
        _syncHealth = GetComponent<SyncHealth>();
        _syncRevive = GetComponent<SyncRevive>();
        Invoke(nameof(ActivateDie), 1);
    }

    private void Update()
    {
        if(_isActivated == false)
            return;
            
        if (_currentHealth <= 0 && isDie == false && isLocalPlayer)
        {
            isDie = true;
            Die();
        }
        if (_currentHealth > 0)
        {
            isDie = false;
        }
    }
    private void Die()
    {
        if (isServer)
        {
            DestroyGameObject();
        }
        else
        {
            CmdDestroyGameObject();
        }
    }
    [Server]
    private void DestroyGameObject()
    {
        RpcDestroyGameObject();
    }
    [Command(requiresAuthority = false)]
    private void CmdDestroyGameObject()
    {
        DestroyGameObject();
    }

    [ClientRpc]
    private void RpcDestroyGameObject()
    {
        _character.Die();

        foreach (var item in _disableUponDeath)
        {
            if (item != null)
                item.SetActive(false);
        }
    }

    private void ActivateDie()
    {
        _isActivated = true;
    }

}
