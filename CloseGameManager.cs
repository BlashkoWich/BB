using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseGameManager : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(FindPlayer());
    }
    private IEnumerator FindPlayer()
    {
        yield return new WaitForSecondsRealtime(120);
        BB.Core.Character[] characters = FindObjectsOfType<BB.Core.Character>();
        if(characters[0] == null)
        {
            Application.Quit();
        }
        StartCoroutine(FindPlayer());
    }
}
