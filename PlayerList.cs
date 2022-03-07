using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerList : NetworkBehaviour
{
    
    [SerializeField]
    private ScoreController _scoreController;

    public Dictionary<Team, List<SetPlayerInfo>> TeamInfo = new Dictionary<Team, List<SetPlayerInfo>>();


    public SyncDictionary<Team, int> EveryTeamPlayerCount = new SyncDictionary<Team, int>();

    private void Awake()
    {
        SetTeam();
    }
    private void Start()
    {
        if(isServer)
            ServerSetTeams();
    }

    private void SetTeam()
    {
        for (int i = 0; i < _scoreController.TeamsInGame.Length; i++)
        {
            List<SetPlayerInfo> setPlayerInfos = new List<SetPlayerInfo>();
            TeamInfo.Add(_scoreController.TeamsInGame[i], setPlayerInfos);
        }
    }

    [Server]
    private void ServerSetTeams()
    {
        for (int i = 0; i < _scoreController.TeamsInGame.Length; i++)
        {
            EveryTeamPlayerCount.Add(_scoreController.TeamsInGame[i], 0);
        }
    }

    public void AddPlayerInTeam(Team team, SetPlayerInfo playerInfo)
    {
        List<SetPlayerInfo> list = TeamInfo[team];
        list.Add(playerInfo);
        TeamInfo[team] = list;  
        
        
    }

    public void AddPlayerForCount(Team team)
    {

        if(isServer)
        {
            ServerAddPlayer(team);
        }
        else
        {
            CmdAddPlayer(team);
        }
    }

    [Command(requiresAuthority = false)]
    private void CmdAddPlayer(Team team)
    {
        ServerAddPlayer(team);
    }
    [Server]
    private void ServerAddPlayer(Team team)
    {
        EveryTeamPlayerCount[team]++;
    }

}
