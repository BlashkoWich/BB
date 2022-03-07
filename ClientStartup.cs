using Mirror;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.MultiplayerModels;
using System.Collections.Generic;
using PlayFab.Networking;
using UnityEngine;

public class ClientStartup : MonoBehaviour 
{
    private void Start()
    {
        LoginWithCustomIDRequest request = new LoginWithCustomIDRequest()
        {
            TitleId = PlayFabSettings.TitleId,
            CreateAccount = true,
            CustomId = SystemInfo.deviceUniqueIdentifier
        };

        PlayFabClientAPI.LoginWithCustomID(request, OnPlayFabLoginSuccess, OnLoginError);
    }

    private void OnPlayFabLoginSuccess(LoginResult loginResult)
    {
        Debug.Log("Login Success");

        RequestMultiplayerServer();
    }

    private void RequestMultiplayerServer()
    {
        Debug.Log("[ClientStartUp].RequestMultiplayerServer");

        RequestMultiplayerServerRequest requestData = new RequestMultiplayerServerRequest
        {
            BuildId = "428d5a7f-d202-476b-8a8b-85759e10610f",
            SessionId = "13A61488-6676-4D9F-A19B-D5B2BFF71310",
            PreferredRegions = new List<string> {"NorthEurope"}
        };

        PlayFabMultiplayerAPI.RequestMultiplayerServer(requestData, OnRequestMultiplayerServer, OnRequestMultiplayerServerError);
    }
    private void OnRequestMultiplayerServer(RequestMultiplayerServerResponse response)
    {
        if(response == null) return;
        Debug.Log("requesik");

        UnityNetworkServer.Instance.networkAddress = response.IPV4Address;
        UnityNetworkServer.Instance.GetComponent<kcp2k.KcpTransport>().Port = (ushort) response.Ports[0].Num;

        UnityNetworkServer.Instance.StartClient();
    }

    private void OnRequestMultiplayerServerError(PlayFabError playFabError)
    {
        Debug.Log("An error occurred." + playFabError);
        RequestMultiplayerServer();
    }

    private void OnLoginError(PlayFabError playFabError)
    {

    }
}