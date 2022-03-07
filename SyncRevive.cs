using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

[RequireComponent(typeof(SyncHealth), typeof(SyncDie))]
[RequireComponent(typeof(BB.Core.Character))]
[RequireComponent(typeof(SelectSpawnPoint))]
public class SyncRevive : NetworkBehaviour
{
    public event Action HealthStorageSetted;
    [SerializeField]
    private SelectSpawnPoint _selecSpawnPoint;
    private GameObject[] _disableUponDeath => _syncDie.DisableUponDeath;

    private ScoreController _scoreController;
    private Vector3 _spawnPoint => _selecSpawnPoint.Spawnpoint;

    [SerializeField]
    private int _timeToRevive;

    private Team _team = Team.Obstacle;

    private SyncDie _syncDie;
    private SyncHealth _syncHealth;
    private DamageReceiving.HealthStorage _healthStorage;
    private int _defaultMaxHealth;
    private BB.Core.Character _character;
    private bool _isAlreadyRevive = false;
    private bool _isGameOver = false;
    public bool IsGameOver => _isGameOver;
    private bool _isAlreadyDie;
    private int _currentScore;
    private bool isActivate;

    private void Start()
    {
        _character = GetComponent<BB.Core.Character>();
        _syncHealth = GetComponent<SyncHealth>();
        _scoreController = FindObjectOfType<ScoreController>();
        _syncDie = GetComponent<SyncDie>();
    }
    public void SetHealthStorage(DamageReceiving.HealthStorage healthStorage)
    {
        _healthStorage = healthStorage;
        _defaultMaxHealth = _healthStorage.MaxHealth;

        HealthStorageSetted?.Invoke();
        isActivate = true;
    }

    private void Update()
    {
        if(isActivate == true)
        {
            SetCantRevive();
            GetTeamScore();
            Revive();
        }
    }

    public void SetTeam(Team team)
    {
        _team = team;
    }

    private void Revive()
    {

        if (_syncHealth.CurrentHealth <= 0 && _isAlreadyRevive == false && isLocalPlayer && _currentScore > 0)
        {
            _isAlreadyRevive = true;

            _character.RevivalAfterSpecialTime();
            StartCoroutine(RevivePlayer());
        }
        if (_syncHealth.CurrentHealth > 0 && isLocalPlayer)
        {
            _isAlreadyRevive = false;
        }
    }
    
        


    private IEnumerator RevivePlayer()
    {
        if (isServer)
        {
            SetMaxHealth();
        }
        
        gameObject.transform.position = _spawnPoint;
        yield return new WaitForSeconds(_timeToRevive);
        
        _character.Revival();

        gameObject.transform.position = _spawnPoint;
        if (isServer)
        {
            ReviveServer();
        }
        else
        {
            CmdRevivePlayer();
        }
    }
    [Server]
    private void SetMaxHealth()
    {
        _syncHealth.RpcSetMaxHealth(_defaultMaxHealth);
    }

    [Server]
    private void ReviveServer()
    {
        _syncHealth.RpcSetMaxHealth(500);
        _syncHealth.ServerChangeHealthValue(500);
        _scoreController.RemoveScore(_team);
        RpcRevivePlayer();
    }

    [Command(requiresAuthority = false)]
    private void CmdRevivePlayer()
    {
        ReviveServer();
    }

    [ClientRpc]
    private void RpcRevivePlayer()
    {
        foreach (var item in _disableUponDeath)
        {
            if (item != null)
                item.SetActive(true);
        }
    }

    private void GetTeamScore()
    {
        foreach (var team in _scoreController.TeamsCountScore)
        {
            if (_team == team.Key)
            {
                _currentScore = team.Value;
            }
        }
    }

    private void SetCantRevive()
    {
        if (_syncHealth.CurrentHealth <= 0 && _isGameOver == false && isLocalPlayer && _currentScore <= 0)
        {
            _isGameOver = true;

            _scoreController.CmdAddAllreadyPlayersDie(_team);
        }
    }
}