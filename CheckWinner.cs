using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckWinner : MonoBehaviour
{
    [SerializeField]
    private ScoreController _scoreController;

    [SerializeField]
    private GameObject _redTeamWinTxt;
    [SerializeField]
    private GameObject _blueTeamWinTxt;
    
    private void Update()
    {
        if(_scoreController.TeamWinner == Team.Red)
        {
            _redTeamWinTxt.SetActive(true);
            _blueTeamWinTxt.SetActive(false);
        }
        else if(_scoreController.TeamWinner == Team.Blue)
        {
            _redTeamWinTxt.SetActive(false);
            _blueTeamWinTxt.SetActive(true);
        }
        else if(_scoreController.TeamWinner == Team.Obstacle)
        {
            Debug.Log("Победила дружба");
        }
    }
}
