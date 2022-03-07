using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class UnderMarkColorSetter : NetworkBehaviour
{
    private Team mainCharacterTeam;
    [SerializeField]
    private Renderer _renderer;

    [SerializeField]
    private Color _myselfColor;
    [SerializeField]
    private Color _enemyColor;
    [SerializeField]
    private Color _teamColor;

    private void Start()
    {
        Invoke(nameof(SetMark), 0.5f);
    }

    public void SetMark()
    {
        if(isLocalPlayer == true)
        {
            _renderer.material.color = _myselfColor;
            return;
        }

        BB.Core.Character[] characters = FindObjectsOfType<BB.Core.Character>();
        for (int i = 0; i < characters.Length; i++)
        {
            if(characters[i].isLocalCharacter == true)
            {
                mainCharacterTeam = characters[i].Team;
            }
        }


        BB.Core.Character thisCharacter = GetComponent<BB.Core.Character>();

        if(thisCharacter.Team == mainCharacterTeam)
        {
            _renderer.material.color = _teamColor;
        }
        else
        {
            _renderer.material.color = _enemyColor;
        }
    }
}
