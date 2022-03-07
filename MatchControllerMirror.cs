using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MatchControllerMirror : NetworkBehaviour
{
    private int _countPlayers;
    [SerializeField]
    private MirrorBeforeMatch[] _mirrorBeforeMatch;
    [SerializeField]
    private MirrorMatchIntro[] _mirrorMatchIntro;

    [SerializeField]
    private OnFinishMatchObject[] _onFinishMatchObjects;

    [SerializeField]
    private ScoreController _scoreController;

    [SerializeField]
    private GameObject _mainPanel;

    [SyncVar]
    private int _timer = 100;
    public int Timer => _timer;
    [SerializeField]
    private BB.UI.Controllers.UIMatchLauncherController _uIMatchLauncherController;

    [SerializeField]
    private Camera _finishCamera;
    [SerializeField]
    private Canvas _gameCanvas;

    private bool _matchIsStarted;
    private bool _isGameOver;
    public bool MathcIsStarted => _matchIsStarted;

    private void Start()
    {
        if(isServer)
        TimerServer();
    }

    public void AddPlayerForCount()
    {
        _countPlayers++;
        if(_countPlayers >= NetworkServer.maxConnections)
        {
            StartPreludeGame();
            foreach (var team in _scoreController.TeamsCountScore)
            {
                if(team.Value == _scoreController.MaxScore)
                {
                    CmdSetDefaultTime();
                }
            }
        }
    }

    [Command(requiresAuthority = false)]
    private void CmdSetDefaultTime()
    {
        SetDefaultTime();
    }
    [Server]
    private void SetDefaultTime()
    {
        _timer = 100;
    }

    private void StartPreludeGame()
    {
        BB.Core.Character[] characters = FindObjectsOfType<BB.Core.Character>();
        foreach (var item in characters)
        {
            item.RevertUI();
        }
        foreach(var obj in _mirrorMatchIntro)
        {
            if(obj != null)
            obj.gameObject.SetActive(true);
        }
        _uIMatchLauncherController.OnCharactersLaunched();
    }

    
    public void OnIntroFinished()
    {
        foreach (var item in _mirrorMatchIntro)
        {
            Destroy(item.gameObject);
        }
        foreach (var item in _mirrorBeforeMatch)
        {
            Destroy(item.gameObject);
        }
        _mainPanel.SetActive(true);
        OnMatchObject[] onMatchObjects = FindObjectsOfType<OnMatchObject>();
        foreach(var obj in onMatchObjects)
        {
            obj.ActivateGameObject();
        }
        _matchIsStarted = true;
    }


    [ClientRpc]
    public void RpcActivateGameOver()
    {

        OnMatchObject[] onMatchObjects = FindObjectsOfType<OnMatchObject>();
        foreach(var obj in onMatchObjects)
        {
            Destroy(obj.gameObject);
        }
        foreach(var obj in _onFinishMatchObjects)
        {
            obj.gameObject.SetActive(true);
        }
        _gameCanvas.worldCamera = _finishCamera;
        _isGameOver = true;
        
        Invoke(nameof(CmdApplicationQuit), 25);
    }
    [Command(requiresAuthority = false)]
    private void CmdApplicationQuit()
    {
        ApplicationQuit();
    }
    [Server]
    public void ApplicationQuit()
    {
        Application.Quit();
    }


    [Server]
    private void TimerServer()
    {
        if(_timer <= 0 || _isGameOver == true) return;
        _timer--;
        StartCoroutine(TimerNumerator());
        if(_timer == 0)
        {
            _scoreController.RpcSelectWinner();
            RpcActivateGameOver();
        }
    }
    private IEnumerator TimerNumerator()
    {
        yield return new WaitForSeconds(1);
        if(isServer)
        {
            TimerServer();
        }
    }

}
