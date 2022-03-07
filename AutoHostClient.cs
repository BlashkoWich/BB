using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class AutoHostClient : MonoBehaviour
{
    [SerializeField]
    private NetworkManager _networkManager;
    public void JoinLocal()
    {
        _networkManager.networkAddress = "localhost";
        _networkManager.StartClient();
    }
}
