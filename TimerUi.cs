using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TimerUi : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _timer;
    [SerializeField]
    private MatchControllerMirror _matchController;

    private void Update()
    {
        _timer.text = _matchController.Timer.ToString();
    }
}
