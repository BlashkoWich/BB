using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BB.StateMachine
{
    public class AttackState : State
    {
        [SerializeField]
        private ParticleSystem _attackEffect;

        public override void OnEnter()
        {
            _attackEffect.Play();
        }

        public override void OnExit()
        {
           // _attackEffect.Stop();
        }

        public override void OnUpdate()
        {

        }
    }
}