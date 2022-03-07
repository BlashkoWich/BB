using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharactersColorableObject : MonoBehaviour
{
    [SerializeField]
    private protected List<Renderer> _renderer = new List<Renderer>();

    [SerializeField]
    private List<TeamColor> _teamsColor = new List<TeamColor>();


    public virtual void SetColor(Color color)
    {
        foreach (var k in _renderer)
        {
            k.material.SetColor("_BaseColor", color);
        }
    }

    public void SetColor(Team team)
    {
        if (_teamsColor.Count == 0)
            return;

        var color = _teamsColor.Where(a => a.Team == team).First().Color;

        SetColor(color);
    }
}
