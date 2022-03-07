using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsPlayer : MonoBehaviour
{
    [SerializeField]
    private SyncHealth _syncHealth;
    public SyncHealth SyncHealth => _syncHealth;
}
