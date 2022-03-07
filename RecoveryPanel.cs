using System.Collections;
using System.Collections.Generic;
using BB.Core;
using BB.Helpers;
using UnityEngine;

public class RecoveryPanel : MonoBehaviour
{
    [SerializeField]
    private TMPro.TextMeshProUGUI _timeToRecovery;

    public void ActivatePanel(float time)
    {
        this.gameObject.SetActive(true);
        StartCoroutine(UIAnimation.UICountDown(_timeToRecovery, 3, 0, 1, () =>
        {
            DeactivatePanel();
        }));
    }

    public void DeactivatePanel()
    {
        this.gameObject.SetActive(false);
    }
}
