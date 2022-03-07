using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Server;

public class SyncDataBase : NetworkBehaviour
{
    private BB.Core.Character _character;
    public BB.Core.Character Character => _character;
    private void Start()
    {
        if(isLocalPlayer == false) 
        {
            Destroy(this);
            return;
        }
        /*
        if(PlayerPrefs.HasKey("login") && PlayerPrefs.HasKey("password"))
        {
            Debug.Log(PlayerPrefs.GetString("login") + " " + PlayerPrefs.GetString("password"));
            SendLoginRequest(PlayerPrefs.GetString("login"), PlayerPrefs.GetString("password"));
        }*/
        _character = GetComponent<BB.Core.Character>();
        SendLoginRequest("testtest", "TestTest");
    }

    private void SendLoginRequest(string login, string password)
    {
        ServerRequest loginRequest = DatabaseRequestBuilder.LoginRequest(login, password);
        loginRequest.OnGetFeedback += (ServerFeedback feedback) =>
        {
            if (feedback.Type == ServerFeedbackType.Success)
            {
                PlayerPrefs.SetString("login", login);
                PlayerPrefs.SetString("password", password);
                UserManager.Instance.GetCurrentData(login);
            }
        };
        ServerManager.Instance.SendRequest(loginRequest);
    }

    public void UpdateCoinRaiting(int newCoin, int newRating)
    {/*
        if(isLocalPlayer == false) return;
         UserData newData = UserManager.Instance.CurrentUser.Clone();
         
            newData.coins = newCoin;
            newData.rating = newRating;

        UserManager.Instance.SetCurrentData(newData);*/
    }
}
