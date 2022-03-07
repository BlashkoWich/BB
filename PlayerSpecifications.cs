using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BB.ScriptableObjects
{
    //здесь находится инофрмация об игроке в сетевом мире
    //Его ник
    //Его уровень
    //количество кубков
    //Уровень каждого персонажа
    //

    [CreateAssetMenu(fileName = "PlayerSpecifications", menuName = "BrawlBrunch/PlayerSpecifications", order = 0)]
    public class PlayerSpecifications : ScriptableObject
    {
        public int CupAmount;

        public string Nickname;
    }
}
