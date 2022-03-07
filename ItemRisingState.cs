using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BB.StateMachine
{
    public class ItemRisingState : State
    {
        [SerializeField]
        private ParticleSystem _risingEffect;
        public override void OnEnter()
        {
            _risingEffect.Play();
        }

        public override void OnExit()
        {

        }

        public override void OnUpdate()
        {

        }
    }
}