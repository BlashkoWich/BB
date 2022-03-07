using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnMatchObject : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _activateOnMatchStart;

    private void Start()
    {
        foreach(var obj in _activateOnMatchStart)
        {
            if(obj != null)
            obj.SetActive(false);
        }
    }
    public void ActivateGameObject()
    {
        foreach(var obj in _activateOnMatchStart)
        {
            if(obj != null)
            obj.SetActive(true);
        }
    }
}
