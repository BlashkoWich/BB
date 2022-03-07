using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterStateMachine
{
    public struct CharacterStateUpdatedInfo
    {
        public CharacterState PrevState;
        public CharacterState CurrentState;
    }
}