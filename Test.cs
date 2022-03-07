using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Test : NetworkBehaviour
{
    private void Start()
    {
        Debug.Log(NetworkServer.maxConnections + "Max Connections");
    }
}
