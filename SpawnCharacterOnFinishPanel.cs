using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class SpawnCharacterOnFinishPanel : NetworkBehaviour
{
    [SerializeField] 
    private PlayerList _playerList;
    [SerializeField]
    private ScoreController _scoreController;

    private Dictionary<Team, List<GameObject>> _spawnpoint = new Dictionary<Team, List<GameObject>>();


    [SerializeField]
    private List<GameObject> _spawnPointsList = new List<GameObject>();
    private bool isSpawned;

    private void Start()
    {
        int countSpawnpointPerTeam = _spawnPointsList.Count / _scoreController.TeamsInGame.Length; 
        for (int i = 0; i < _scoreController.TeamsInGame.Length; i++)
        {
            List<GameObject> spawnPointTeam = new List<GameObject>();
            for (int y = 0; y < countSpawnpointPerTeam; y++)
            {
                spawnPointTeam.Add(_spawnPointsList[0]);
                _spawnPointsList.RemoveAt(0);
            }
            
            _spawnpoint.Add(_scoreController.TeamsInGame[i], spawnPointTeam);
        }
    }
    
    private void Update()
    {
        
        if(isSpawned == true) return;

        int currentPlayers = 0;
        foreach (var team in _playerList.TeamInfo)
        {
            currentPlayers += team.Value.Count;
        }

        if(currentPlayers >= NetworkServer.maxConnections && isSpawned == false)
        {
            
            isSpawned = true;
            for (int i = 0; i < _scoreController.TeamsInGame.Length; i++)
            {
                Team team = _scoreController.TeamsInGame[i];
                List<SetPlayerInfo> playerList = _playerList.TeamInfo[team];
                List<GameObject> spawnpoints = _spawnpoint[team];
                for (int y = 0; y < playerList.Count; y++)
                {
                    GameObject modelPlayer = playerList[y].Character.ModelForFinishPanel;
                    Instantiate(modelPlayer, spawnpoints[y].transform.position, Quaternion.Euler(0, 180, 0));
                }
            }
            
        }
    }
}
