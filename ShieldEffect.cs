using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldEffect : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> _shields = new List<GameObject>();

    [SerializeField]
    private List<Renderer> _renderers = new List<Renderer>();

    [SerializeField]
    private Collider _collider;

    [SerializeField]
    private DamageReceiving.DamageReceiver _damageReceiver;

    [SerializeField]
    private DamageReceiving.HealthUI _healthUI;

    void OnEnable()
    {
        _damageReceiver.DamageTouched += OnDamageTouched;
    }
    void OnDisable()
    {
        _damageReceiver.DamageTouched -= OnDamageTouched;
    }

    private void OnDamageTouched(DamageDealer damageDealer)
    {
        damageDealer.OnDamageDelievered();
    }


    public void RotateShields()
    {
        _healthUI.SetImmortalColor();
        foreach (var shield in _shields)
        {
            shield.gameObject.SetActive(true);
        }
        _collider.enabled = true;
        StartCoroutine(Rotating());
        StartCoroutine(AlphaChanging());
    }

    private IEnumerator Rotating()
    {
        for (int i = 0; i < 200; i++)
        {
            transform.localEulerAngles += Vector3.up;
            yield return new WaitForSecondsRealtime(0.01f);
        }
    }

    private IEnumerator AlphaChanging()
    {
        foreach (var shield in _renderers)
        {
            var color = shield.material.color;
            shield.material.color = new Color(color.r, color.g, color.b, 0);
        }

        for (int i = 0; i <= 50; i++)
        {
            foreach (var shield in _renderers)
            {
                var color = shield.material.color;
                shield.material.color = new Color(color.r, color.g, color.b, (float)(i / 50.0f));
            }
            yield return new WaitForSecondsRealtime(0.01f);
        }
        for (int i = 50; i >= 0; i--)
        {
            foreach (var shield in _renderers)
            {
                var color = shield.material.color;
                shield.material.color = new Color(color.r, color.g, color.b, (float)(i / 50.0f));
            }
            yield return new WaitForSecondsRealtime(0.01f);
        }
        for (int i = 0; i <= 50; i++)
        {
            foreach (var shield in _renderers)
            {
                var color = shield.material.color;
                shield.material.color = new Color(color.r, color.g, color.b, (float)(i / 50.0f));
            }
            yield return new WaitForSecondsRealtime(0.01f);
        }
        for (int i = 50; i >= 0; i--)
        {
            foreach (var shield in _renderers)
            {
                var color = shield.material.color;
                shield.material.color = new Color(color.r, color.g, color.b, (float)(i / 50.0f));
            }
            yield return new WaitForSecondsRealtime(0.01f);
        }

        foreach (var shield in _shields)
        {
            shield.gameObject.SetActive(false);
        }

        _collider.enabled = false;
        _healthUI.SetStandardColor();
    }
}
