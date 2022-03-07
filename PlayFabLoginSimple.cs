using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

public class PlayFabLoginSimple : MonoBehaviour
{

    public static string SessionTicket;
    public static string EntityID;

    private void Start()
    {
        LoginPlayfab();
    }

    private void LoginPlayfab()
    {
        var request = new LoginWithCustomIDRequest
        {
            CustomId = System.Guid.NewGuid().ToString(),
            CreateAccount = true
        };

        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginFailure);
    }

    private void OnLoginSuccess(LoginResult result)
    {
        Debug.Log("Loggin Success");
        SessionTicket = result.SessionTicket;
        EntityID = result.EntityToken.Entity.Id;
    }
    private void OnLoginFailure(PlayFabError error)
    {
        Debug.LogError(error.GenerateErrorReport());
    }
}
