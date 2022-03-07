using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BB.StateMachine
{
    public class RevivalState : State
    {
        [SerializeField]
        private CharacterVisual.VisualHider _visualHider;

        [SerializeField]
        private ShieldEffect _shieldEffect;

        [SerializeField]
        private DissolveEffect _dissolveEffect;

        [SerializeField]
        private RecoveryPanel _recoveryPanel;

        public override void OnEnter()
        {
            if(_recoveryPanel != null)
            _recoveryPanel.ActivatePanel(3);
        }

        public override void OnExit()
        {
            _visualHider.SetVisibility(true);
            _dissolveEffect.RevivalEffectAnimation();
            _shieldEffect.RotateShields();
        }

        public override void OnUpdate()
        {

        }
    }
}