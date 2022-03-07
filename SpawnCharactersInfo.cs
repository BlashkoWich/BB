using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class SpawnCharactersInfo : NetworkBehaviour
{
    [SerializeField]
    public List<GameObject> Rangers;

    [SerializeField]
    public List<GameObject> BlueTeamSpawnPoints;
    [SerializeField]
    public List<GameObject> RedTeamSpawnPoints;

    [Server]
    public void Remove()
    {
        Rangers.RemoveAt(0);
        BlueTeamSpawnPoints.RemoveAt(0);
    }
}
