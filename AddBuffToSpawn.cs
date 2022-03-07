using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddBuffToSpawn : MonoBehaviour
{
    [SerializeField]
    private Buffs.BuffKeeper _buffKeeper;

    public Buffs.BuffKeeper BuffKeeper => _buffKeeper;

    public void AddToSpawn()
    {
        _buffKeeper.AddBuffToSpawn();
    }

}
