using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class SetGrassCharacter : NetworkBehaviour
{
    private BB.Core.Character _character;
    private GrassHider[] _allGrass;

    private void Start()
    {
        if(isLocalPlayer == false) return;
        _character = GetComponent<BB.Core.Character>();
        _allGrass = FindObjectsOfType<GrassHider>();
        foreach(var grass in _allGrass)
        {
            grass._teamLocalPlayer = _character.Team;
        }
    }
}
