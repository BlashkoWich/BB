using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BB.StateMachine
{
    public class RecoveryState : State
    {

        [SerializeField]
        private List<ParticleSystem> _recoveryPS;

        public override void OnEnter()
        {
            foreach (var ps in _recoveryPS)
            {
                ps.Play();
            }
        }

        public override void OnExit()
        {
            foreach (var ps in _recoveryPS)
            {
                ps.Stop();
            }
        }

        public override void OnUpdate()
        {

        }
    }
}