using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using BB.Buffs;
using System;

public class SyncHealth : NetworkBehaviour, IBuffReactor
{
    public event Action HealthStorageSetted;
    private int _currentHealth;
    public int CurrentHealth => _currentHealth;
    public int HealthStorageCurrentHealth => _healthStorage.CurrentHealth;

    private int _defaultMaxHealth;
    public int MaxHealth => _healthStorage.MaxHealth;
    private DamageReceiving.HealthStorage _healthStorage;

    private void Update()
    {
        if (_healthStorage != null)
            _healthStorage.SetHealthValue(_currentHealth);
    }

    public void ChangeHealth(int newHealthValue)
    {
        if(isLocalPlayer == true)
        return;
        if (newHealthValue < 0)
        {
            int deltaForMaxHealth = _healthStorage.MaxHealth - _currentHealth;
            newHealthValue = Mathf.Clamp(newHealthValue, -deltaForMaxHealth, 0);
        }
        if (isServer)
        {
            ServerChangeHealthValue(_currentHealth - newHealthValue);

        }
        else
        {
            CmdChangeHealth(_currentHealth - newHealthValue); //в противном случае делаем на сервер запрос об изменении переменной
        }
    }


    [Command(requiresAuthority = false)] //обозначаем, что этот метод должен будет выполняться на сервере по запросу клиента
    private void CmdChangeHealth(int newValue) //обязательно ставим Cmd в начале названия метода
    {
        ServerChangeHealthValue(newValue); //переходим к непосредственному изменению переменной
    }

    [Server] //обозначаем, что этот метод будет вызываться и выполняться только на сервере
    public void ServerChangeHealthValue(int newValue)
    {
        RpcChangeHealthValue(newValue);
    }

    [ClientRpc]
    private void RpcChangeHealthValue(int newValue)
    {
        _currentHealth = newValue;
    }

    public void SetHealthStorage(DamageReceiving.HealthStorage healthStorage)
    {
        _healthStorage = healthStorage;
        _currentHealth = _healthStorage.MaxHealth;
        _defaultMaxHealth = _healthStorage.MaxHealth;

        HealthStorageSetted?.Invoke();
    }

    public void ApplyBuff(Buff buff)
    {
        if (isLocalPlayer == false) return;
        int extraHitPoints = buff.ExtraHitPoints; // количество дополнительных очков здоровья 


        Debug.Log(extraHitPoints + "   доп здоровье с бафа");
        BuffHealth(extraHitPoints);
    }

    private void BuffHealth(int newHealthValue)
    {
        if (isServer)
        {
            ServerChangeHealthValue(_currentHealth + newHealthValue);
            ServerSetMaxHealth(_healthStorage.MaxHealth + newHealthValue);
        }
        else
        {
            CmdChangeHealth(_currentHealth + newHealthValue); //в противном случае делаем на сервер запрос об изменении переменной

            CmdSetMaxHealth(_healthStorage.MaxHealth + newHealthValue);
        }
    }
    [Command(requiresAuthority = false)]
    private void CmdSetMaxHealth(int newHealthValue)
    {
        ServerSetMaxHealth(newHealthValue);
    }
    [Server]
    private void ServerSetMaxHealth(int newHealthValue)
    {
        RpcSetMaxHealth(newHealthValue);
    }
    [ClientRpc]
    public void RpcSetMaxHealth(int newHealthValue)
    {
        _healthStorage.SetMaxHealth(newHealthValue);
    }

    public void CancelBuff(Buff buff)
    {
        if (isLocalPlayer == false) return;
        var extraHitPoints = buff.ExtraHitPoints; // количество дополнительных очков здоровья 
        Debug.Log(extraHitPoints + "   снять с бафа");
        BuffHealth(-extraHitPoints);
    }
}
