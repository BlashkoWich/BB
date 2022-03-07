using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class HideGrassMirror : NetworkBehaviour
{
    [SerializeField]
    private MeshRenderer[] _modelPlayer;
    [SerializeField]
    private GameObject[] _playerUI;

    private BB.Core.Character _character;

    private bool isHide = false;

    private void Start()
    {
        _character = GetComponent<BB.Core.Character>();
    }

    public void SetHide(bool activator)
    {
        if(isServer)
        {
            ServerSetHide(activator);
        }
        else
        {
            CmdSetHide(activator);
        }
    }

    [Command(requiresAuthority = false)]
    private void CmdSetHide(bool activator)
    {
        ServerSetHide(activator);
    }
    [Server]
    private void ServerSetHide(bool activator)
    {
        RpcSetHide(activator);
    }
    [ClientRpc]
    private void RpcSetHide(bool activator)
    {
        isHide = activator;
        if(isLocalPlayer == false) 
        {
            if(isHide == true)
            {
                foreach(var mesh in _modelPlayer)
                {
                    mesh.enabled = false;
                }
                foreach(var ui in _playerUI)
                {
                    ui.SetActive(false);
                }
            }
            else
            {
                foreach(var mesh in _modelPlayer)
                {
                    mesh.enabled = true;
                }
                foreach(var ui in _playerUI)
                {
                    ui.SetActive(true);
                }
            }
        }
    }
}
