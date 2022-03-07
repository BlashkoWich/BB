using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ScoreController : NetworkBehaviour
{
    public event Action<Team, int> TeamScoreChanged;


    [SerializeField]
    private Team[] _teams;
    public Team[] TeamsInGame => _teams;
    

    private SyncDictionary<Team, int> _teamsScore = new SyncDictionary<Team, int>();
    public SyncDictionary<Team, int> TeamsCountScore => _teamsScore;
    
    [SerializeField]
    private int _maxScore;
    public int MaxScore => _maxScore;

    private SyncDictionary<Team, int> AllreadyTeamPlayersDie = new SyncDictionary<Team, int>();

    [SerializeField]
    private MatchControllerMirror _matchController;

    private Team _teamWinner;
    public Team TeamWinner => _teamWinner;

    private SyncDataBase _syncDataBase;
    [SerializeField]
    private PlayerList _playerList;

   private void Start()
    {
        if(isServer)
        {
            SetStartScore();
        }
    }

    [Server]
    private void SetStartScore()
    {
        for (int i = 0; i < _teams.Length; i++)
        {
            _teamsScore.Add(_teams[i], _maxScore);
            AllreadyTeamPlayersDie.Add(_teams[i], 0);
        } 
    }

    public void RemoveScore(Team team)
    {
        if(isServer)
        {
            ServerRemoveScore(team);
        }
        else
        {
            CmdRemoveScore(team);
        }
    }
    [Command(requiresAuthority = false)]
    private void CmdRemoveScore(Team team)
    {
        ServerRemoveScore(team);
    }
    [Server]
    private void ServerRemoveScore(Team team)
    {
        int value = _teamsScore[team];
        Debug.Log(value-- + "велью - 1");
        _teamsScore[team] = value--;
        RpcRemoveScore(team);
    }
    [ClientRpc]
    private void RpcRemoveScore(Team team)
    {
        TeamScoreChanged?.Invoke(team, 1);
    }



    [ClientRpc]
    public void RpcSelectWinner()
    {
        int maxValue = 0;
        foreach (var team in _teamsScore)
        {
            if(team.Value > maxValue)
            {
                maxValue = team.Value;
                _teamWinner = team.Key;
            }
            else if(team.Value == maxValue)
            {
                _teamWinner = Team.Draw;
            }
        }
        CountCointAndRaiting();
    }
    [ClientRpc]
    public void RpcSetTeamWinner(Team teamLoser)
    {
        foreach (var team in _teams)
        {
            if(team != teamLoser)
            {
                _teamWinner = team;
            }
        }
        CountCointAndRaiting();
    }

    private void CountCointAndRaiting()
    {
        
        _syncDataBase = FindObjectOfType<SyncDataBase>();
        if(_teamWinner == _syncDataBase.Character.Team)
        {
            _syncDataBase.UpdateCoinRaiting(100, 10);
        }
        else if(_teamWinner == Team.Draw)
        {
            _syncDataBase.UpdateCoinRaiting(50, 0);
        }
        else
        {
            _syncDataBase.UpdateCoinRaiting(10, -10);
        }
    }


    [Command(requiresAuthority = false)]
    public void CmdAddAllreadyPlayersDie(Team team)
    {
        AddAllreadyPlayersDie(team);
    }

    [Server]
    private void AddAllreadyPlayersDie(Team team)
    {
        for (int i = 0; i < _teams.Length; i++)
        {
            AllreadyTeamPlayersDie[team] += 1;
            int countThisTeamActivePlayer = 0;
            BB.Core.Character[] playersCount = FindObjectsOfType<BB.Core.Character>();
            foreach(var player in playersCount)
            {
                if(player.Team != team)
                {
                    countThisTeamActivePlayer++;
                }
            }
            if(countThisTeamActivePlayer >= AllreadyTeamPlayersDie[team])
            {
                RpcSetTeamWinner(team);
                _matchController.RpcActivateGameOver();
            }
        }
    }
}
