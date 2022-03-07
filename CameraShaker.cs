using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraShaker : MonoBehaviour
{
    public static CameraShaker Instance { get; private set; }

    private Camera _virtualCamera;

    private CinemachineBasicMultiChannelPerlin _cinemachineBasicMultiChannelPerlin;
    private float _shakeTimer;
    private float _shakeTimerTotal;
    private float _startingIntensity;

    void Awake()
    {
        Instance = this;
        _virtualCamera = GetComponent<Camera>();
    }
    public void Shake(float intensity, float time)
    {
        _cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity;

        _startingIntensity = intensity;
        _shakeTimer = time;
        _shakeTimerTotal = time;
    }

    void Update()
    {
        if (_shakeTimer > 0)
        {
            _shakeTimer -= Time.deltaTime;
            _cinemachineBasicMultiChannelPerlin.m_AmplitudeGain =
                Mathf.Lerp(_startingIntensity, 0f, 1 - (_shakeTimer / _shakeTimerTotal));
        }
    }
}
