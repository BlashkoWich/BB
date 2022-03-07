using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class SetPlayerInfo : NetworkBehaviour
{
    [SerializeField]
    public BB.Core.Character Character;

    public PlayerInfo PlayerInfo;
    public PlayerCharacter PlayerCharacter;
    public Team Team;

    private void Start()
    {
           
    }

    public void Initialize(Team team)
    {
        
        PlayerInfo.nickname = "Челентано";
        Team = team;


        PlayerList playerList = FindObjectOfType<PlayerList>();
        playerList.AddPlayerInTeam(Team, this);
        
        if(isLocalPlayer)
        {
            playerList.AddPlayerForCount(Team);
        }
        MatchControllerMirror matchControllerMirror = FindObjectOfType<MatchControllerMirror>();
        matchControllerMirror.AddPlayerForCount();
    }
}
