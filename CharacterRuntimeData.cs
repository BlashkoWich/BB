using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct CharacterRuntimeData
{
    public Vector3 Position;
    public Quaternion Rotation;
    public bool IsHiding;
    public CharacterStateMachine.CharacterState CharacterState;
    public int AmountRaisedCrystalls;
}
