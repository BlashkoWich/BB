using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class FindPlayers : NetworkBehaviour
{
    private void Start()
    {

    }

    private IEnumerator FindPlayerAndQuit()
    {
        yield return new WaitForSeconds(10);
        BB.Core.Character[] characters = FindObjectsOfType<BB.Core.Character>();
        if(characters.Length == 0 && isServer)
        {
            Application.Quit();
        }
    }
}
