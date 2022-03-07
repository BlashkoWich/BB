using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BB.Core;
using BB.ScriptableObjects;
using UnityEngine;

namespace BB.Weapon
{
    public class UltimateManager : MonoBehaviour
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
                        _character.UpdateUltimateAim(new Vector3(joystickValue.x, 0, joystickValue.y));
                    }
                    else
                    {
                        _character.TakeUltimateAim();
                        _joystickTapped = true;
                    }
                }
                else
                {
                    if (!_joystickInAutoZone)
                    {
                        _joystickInAutoZone = true;
                        _character.HideUltimateAim();
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
        }

        private void OnJoystickUpped(Vector2 obj)
        {
            if (_joystickInAutoZone && !_joystickExitedAutoZone)
            {
                _character.UltimateAutoAtack();
            }
            if (!_joystickInAutoZone && _joystickExitedAutoZone)
            {
                _character.UltimateAttack();
            }
            if (!_joystickInAutoZone && !_joystickExitedAutoZone)
            {
                throw
                    new System.Exception("Joystick not in autozone and didnt exit autozone at the same time");
            }

            _character.RemoveUltimateAim();
            _joystickInAutoZone = true;
            _joystickExitedAutoZone = false;
        }

        private void OnJoystickDowned()
        {

        }
    }
}