using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterStateMachine
{
    public enum CharacterState
    {
        Attack,
        Idle,
        Run,
        Ultimate,
        Die,
        Revival,
        Hiding,
        Recovery,
        DamageReceiving,
        ItemRising
    }
}