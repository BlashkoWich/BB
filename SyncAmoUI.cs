using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;

public class SyncAmoUI : NetworkBehaviour
{
    private List<Slider> _amoSliders = new List<Slider>();
    private BB.Weapon.AmoUI _amoUi;

    private List<float> _valueAmoSliders => _amoUi.GetSliderValues;

    private void Start()
    {
        _amoUi = GetComponentInChildren<BB.Weapon.AmoUI>();
        _amoSliders = _amoUi.GetSliders;
    }

    public void GetSliderValues(int numberOfSlider, float currentSliderValue)
    {
        if(isServer)
        {
            SetSliderValue(numberOfSlider, currentSliderValue);
        }
        else
        {
            CmdSetSliderValue(numberOfSlider, currentSliderValue);
        }
    }

    [Command(requiresAuthority = false)]
    private void CmdSetSliderValue(int numberOfSlider, float currentSliderValue)
    {
        SetSliderValue(numberOfSlider, currentSliderValue);
    }
    [Server]
    private void SetSliderValue(int numberOfSlider, float currentSliderValue)
    {
        RpcSetSliderValue(numberOfSlider, currentSliderValue);
    }
    [ClientRpc]
    private void RpcSetSliderValue(int numberOfSlider, float currentSliderValue)
    {
        _amoSliders[numberOfSlider].value = currentSliderValue;
    }
    
}
