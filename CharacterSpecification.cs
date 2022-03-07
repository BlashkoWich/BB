using System.Collections;
using System.Collections.Generic;
using BB.Core;
using UnityEngine;

namespace BB.ScriptableObjects
{
    //здесь находится инофрмация об персонаже
    //Его иконка
    //Его моделька
    //Его анимации
    //Его звуки
    //

    [CreateAssetMenu(fileName = "CharacterSpecifications", menuName = "BrawlBrunch/CharacterSpecifications", order = 0)]
    public class CharacterSpecification : ScriptableObject
    {
        public new string Name;

        public GameObject Model;

        public Character Character;

        public Texture2D Icon;

        public float StandardSpeed;

        public int MaximumHealth = 100;

        public int HealthRefillValue = 1;
    }
}
