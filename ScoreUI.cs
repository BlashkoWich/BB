using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreUI : MonoBehaviour
{


    [SerializeField]
    private Slider _blueTeamSlider;
    [SerializeField]
    private TextMeshProUGUI _blueTeamScoreTxt;


    [SerializeField]
    private Slider _redTeamSlider;
    [SerializeField]
    private TextMeshProUGUI _redTeamScoreTxt;

    private Dictionary<Team, Slider> _teamsSliders = new Dictionary<Team, Slider>();
    private Dictionary<Team, TextMeshProUGUI> _teamsScoreTxt = new Dictionary<Team, TextMeshProUGUI>();


    [SerializeField]
    private ScoreController _scoreController;

    private void Start()
    {
        _teamsSliders.Add(Team.Red, _redTeamSlider);
        _teamsSliders.Add(Team.Blue, _blueTeamSlider);
        _teamsScoreTxt.Add(Team.Red, _redTeamScoreTxt);
        _teamsScoreTxt.Add(Team.Blue, _blueTeamScoreTxt);

        _redTeamSlider.maxValue = _scoreController.MaxScore;
        _redTeamSlider.value = _scoreController.MaxScore;
        _blueTeamSlider.maxValue = _scoreController.MaxScore;
        _blueTeamSlider.value = _scoreController.MaxScore;
        _redTeamScoreTxt.text = _scoreController.MaxScore.ToString();
        _blueTeamScoreTxt.text = _scoreController.MaxScore.ToString();
    }

    private void OnEnable()
    {
        _scoreController.TeamScoreChanged += OnTeamLoseHitPoint;
    }

    private void OnDisable()
    {
        _scoreController.TeamScoreChanged -= OnTeamLoseHitPoint;
    }
    
    private void OnTeamLoseHitPoint(Team team, int newPoints)
    {
        _teamsSliders[team].value -= newPoints;
        _teamsScoreTxt[team].text = _teamsSliders[team].value.ToString();
    }
}
