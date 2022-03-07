using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharactersColorPicker : MonoBehaviour
{
    [SerializeField]
    private List<CharactersColorableObject> _colorableObjects = new List<CharactersColorableObject>();

    public void PickColor(Color color)
    {
        foreach (var k in _colorableObjects)
        {
            k.SetColor(color);
        }
    }

    public void PickColor(Team team)
    {
        foreach (var k in _colorableObjects)
        {
            k.SetColor(team);
        }
    }
}
