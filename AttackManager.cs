using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BB.Core;
using BB.ScriptableObjects;
using UnityEngine;

namespace BB.Weapon
{
    public class AttackManager : MonoBehaviour
    {
        [SerializeField]
        private JoystickInputHandler _joystickInputHandler;

        [SerializeField]
        private ManagerSettings _inputSettings;

        [SerializeField]
        private BB.Core.Character _character;
        private Vector3 _lastDirection;

        private bool _joystickExitedAutoZone = false;

        private bool _joystickInAutoZone = true;

        private bool _joystickTapped = false;

        void OnEnable()
        {
            _joystickInputHandler.JoystickUpdated += OnJoystickUpdated;
            _joystickInputHandler.JoystickDowned += OnJoystickDowned;
            _joystickInputHandler.JoystickUpped += OnJoystickUpped;

            _character.CharacterStopped += OnCharacterStopped;
        }

        void OnDisable()
        {
            _joystickInputHandler.JoystickUpdated -= OnJoystickUpdated;
            _joystickInputHandler.JoystickDowned -= OnJoystickDowned;
            _joystickInputHandler.JoystickUpped -= OnJoystickUpped;

            _character.CharacterStopped -= OnCharacterStopped;
        }
        private void OnCharacterStopped()
        {
            enabled = false;
        }

        private void OnJoystickUpdated(Vector2 obj)
        {
            var joystickValue = new Vector2(obj.x, obj.y);


            if (_joystickExitedAutoZone)
            {
                if (joystickValue.magnitude * 100.0f > _inputSettings.MinimalPercentageToDrawAim)
                {
                    _joystickInAutoZone = false;

                    if (_joystickTapped)
                    {
                        _character.UpdateAim(new Vector3(joystickValue.x, 0, joystickValue.y));
                    }
                    else
                    {
                        _character.TakeAim();
                        _joystickTapped = true;
                    }

                }
                else
                {
                    if (!_joystickInAutoZone)
                    {
                        _joystickInAutoZone = true;
                        _character.HideAim();
                        _joystickTapped = false;
                    }
                }
            }
            else
            {
                if (joystickValue.magnitude * 100.0f > _inputSettings.MinimalPercentageToDrawAim)
                {
                    _joystickExitedAutoZone = true;
                }
            }


            _lastDirection = new Vector3(obj.x, 0, obj.y);
        }

        private void OnJoystickUpped(Vector2 obj)
        {
            if (_joystickInAutoZone && !_joystickExitedAutoZone)
            {
                _character.AutoAtack();
            }
            if (!_joystickInAutoZone && _joystickExitedAutoZone)
            {
                _character.Attack();
            }
            if (!_joystickInAutoZone && !_joystickExitedAutoZone)
            {
                throw
                    new System.Exception("Joystick not in autozone and didnt exit autozone at the same time");
            }

            _character.RemoveAim();
            _joystickInAutoZone = true;
            _joystickExitedAutoZone = false;
            _joystickTapped = false;
        }

        private void OnJoystickDowned()
        {

        }
    }
}