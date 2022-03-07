using System;
using System.Collections;
using System.Collections.Generic;
using BB.Weapon;
using UnityEngine;

public class UnderUltimateMark : MonoBehaviour
{
    [SerializeField]
    private Aimmer _aimmer;

    [SerializeField]
    private UltimateReloader _reloader;

    [SerializeField]
    private UltLoadingController _ultLoadingController;

    [SerializeField]
    private Renderer _mark;

    [SerializeField]
    private Color _aimmingColor;

    [SerializeField]
    private Color _chargedColor;


    private bool _isActive = false;
    private Vector3 _standardScale;

    private void Awake()
    {
        _standardScale = _mark.gameObject.transform.localScale;
    }

    private void OnEnable()
    {
        _aimmer.Activated += ShowAimmingMark;
        _aimmer.Hided += ShowChargedMark;

        _aimmer.Deactivated += ShowChargedMark;

        _ultLoadingController.FullFilled += Activate;

        _reloader.Reloaded += Deactivate;
    }
    private void OnDisable()
    {
        _aimmer.Activated -= ShowAimmingMark;
        _aimmer.Hided -= ShowChargedMark;

        _aimmer.Deactivated -= ShowChargedMark;

        _ultLoadingController.FullFilled -= Activate;

        _reloader.Reloaded -= Deactivate;
    }

    private void Update()
    {
        if (!_isActive)
            return;

        Rotate();
    }

    private void Activate()
    {
        _isActive = true;
        _mark.gameObject.SetActive(true);
        ShowChargedMark();
    }

    private void Deactivate()
    {
        _isActive = false;
        HideMark();
    }

    private void ShowAimmingMark()
    {
        if (!_isActive)
            return;

        StopAllCoroutines();
        StartCoroutine(Appearing(_aimmingColor));
    }

    private void ShowChargedMark()
    {
        if (!_isActive)
            return;

        StopAllCoroutines();
        StartCoroutine(Appearing(_chargedColor));
    }

    private void HideMark()
    {
        StopAllCoroutines();
        _mark.gameObject.SetActive(false);
    }


    private void Rotate()
    {
        _mark.transform.localEulerAngles += Vector3.up;
    }

    private IEnumerator Appearing(Color color)
    {
        _mark.material.color = color;

        var part = _standardScale * 0.02f;

        _mark.gameObject.transform.localScale = _standardScale * 0.8f;

        for (int i = 0; i < 10; i++)
        {
            _mark.gameObject.transform.localScale += part;
            yield return new WaitForSecondsRealtime(0.01f);
        }
    }
}
