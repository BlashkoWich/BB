using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class SelectSpawnPoint : NetworkBehaviour
{
    private Vector3 _spawnpoint;
    public Vector3 Spawnpoint => _spawnpoint;

    private BB.Core.Character _character;
    
    
    private void Start()
    {
        FindSpawnpoint();
    }
    private void FindSpawnpoint()
    {
        if(isLocalPlayer == true)
        {
            _character = GetComponent<BB.Core.Character>();
            SpawnPoint[] spawnPoints = FindObjectsOfType<SpawnPoint>();
            foreach (var spawnPoint in spawnPoints)
            {
                if(spawnPoint.team == _character.Team && spawnPoint.isOccupied == false)
                {
                    _spawnpoint = spawnPoint.transform.position;
                    transform.position = _spawnpoint;
                    OccupySpawnpoint(spawnPoint);
                    return;
                }
            }
        }
    }
    private void OccupySpawnpoint(SpawnPoint spawnPoint)
    {
        if(isServer)
        {
            spawnPoint.ServerIsOccupied();
        }
        else
        {
            spawnPoint.CmdIsOccupied();
        }
    }
}
