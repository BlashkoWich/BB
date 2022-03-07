using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;

public class PlayFabLogin : MonoBehaviour
{
    [SerializeField]
    private GameObject _authenticationPanel;

    [SerializeField]
    private TMP_InputField _usernameInputField = default;
    [SerializeField]
    private TMP_InputField _emailInputField = default;
    [SerializeField]
    private TMP_InputField _passwordInputField = default;

    public static string SessionTicket;
    public static string EntityID;
    
    public void CreateAccount()
    {
        PlayFabClientAPI.RegisterPlayFabUser(new RegisterPlayFabUserRequest
        {
            Username = _usernameInputField.text,
            Email = _emailInputField.text,
            Password = _passwordInputField.text

        }, result =>
        {
            SessionTicket = result.SessionTicket;
            EntityID = result.EntityToken.Entity.Id;
            _authenticationPanel.SetActive(false);
        }, error =>
        {
            Debug.LogError(error.GenerateErrorReport());
        });
    }
    public void SignIn()
    {
        PlayFabClientAPI.LoginWithPlayFab(new LoginWithPlayFabRequest
        {
            Username = _usernameInputField.text,
            Password = _passwordInputField.text
        }, result =>
        {
            SessionTicket = result.SessionTicket;
            EntityID = result.EntityToken.Entity.Id;
            _authenticationPanel.SetActive(false);
        }, error =>
        {
            Debug.LogError(error.GenerateErrorReport());
        });
    }
}
