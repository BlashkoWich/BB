using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterColorParticleSystem : CharactersColorableObject
{
    public bool IsTrail = false;
    public bool IsEmission = false;

    private string _propertyName;

    public override void SetColor(Color color)
    {
        if (IsEmission)
        {
            _propertyName = "_EmissionColor";
        }
        else
        {
            _propertyName = "_BaseColor";
        }

        if (IsTrail)
        {
            foreach (var k in _renderer)
            {
                k.materials[1].SetColor(_propertyName, color);
            }
        }
        foreach (var k in _renderer)
        {
            k.materials[1].SetColor("_BaseColor", color);
        }
    }
}
