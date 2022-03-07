using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BB.StateMachine
{
    public class UltimateState : State
    {
        [SerializeField]
        private ParticleSystem _ultimateEffect;

        public float ShakeTimerTotal = 0.0f;

        public override void OnEnter()
        {
            Debug.Log("ХУЙ");
            if (_ultimateEffect != null)
                _ultimateEffect.Play();
            CameraShaker.Instance.Shake(0.05f, 1);
        }

        public override void OnExit()
        {
            if (_ultimateEffect != null)
                _ultimateEffect.Stop();
        }

        public override void OnUpdate()
        {

        }
    }
}