using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using BB.Core;

public class TeamColorSetter : MonoBehaviour
{
    [SerializeField]
    public List<TeamColor> TeamColors = new List<TeamColor>();

    private Dictionary<Team, Color> _teamColors = new Dictionary<Team, Color>();
    private Dictionary<Team, List<CharactersColorPicker>> _teamsCharactersColorPickers
        = new Dictionary<Team, List<CharactersColorPicker>>();

    void Awake()
    {
        foreach (var k in TeamColors)
        {
            _teamColors.Add(k.Team, k.Color);
        }

        _teamColors.Distinct();
    }

    public void Initialize(Dictionary<Team, List<Character>> teamsCharactersColorPickers)
    {
        foreach (var k in teamsCharactersColorPickers)
        {
            _teamsCharactersColorPickers.Add(k.Key,
                k.Value.Select(a => a.GetComponent<CharactersColorPicker>()).ToList());
        }
    }

    public void DistributeColors()
    {
        foreach (var k in _teamsCharactersColorPickers)
        {
            //var color = _teamColors[k.Key];

            foreach (var t in k.Value)
            {
                t.PickColor(k.Key);
            }
        }
    }
}

[Serializable]
public struct TeamColor
{
    public Team Team;
    public Color Color;

    public bool Equals(TeamColor other)
    {
        if (other.Team == Team) return true;

        return false;
    }
}
