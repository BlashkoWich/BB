using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class DestroyNotLocalObjects : NetworkBehaviour
{
    [SerializeField] private GameObject[] _gameObjectsForDestroy;

    private void Start()
    {
        if(isLocalPlayer == false) 
        {
            foreach(var obj in _gameObjectsForDestroy)
            {
                Destroy(obj);
            }
        }
    }
}
