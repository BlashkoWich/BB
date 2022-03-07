using System;
using System.Collections;
using System.Collections.Generic;
using BB.ScriptableObjects;
using UnityEngine;

namespace BB.Weapon
{
    public class MovementManager : MonoBehaviour
    {
        [SerializeField]
        private JoystickInputHandler _joystickInputHandler;

        [SerializeField]
        private ManagerSettings _inputSettings;
        [SerializeField]
        private BB.Core.Character _character;

        void OnEnable()
        {
            _joystickInputHandler.JoystickUpdated += OnJoystickUpdated;
            _joystickInputHandler.JoystickDowned += OnJoystickDowned;
            _joystickInputHandler.JoystickUpped += OnJoystickUpped;
        }

        void OnDisable()
        {
            _joystickInputHandler.JoystickUpdated -= OnJoystickUpdated;
            _joystickInputHandler.JoystickDowned -= OnJoystickDowned;
            _joystickInputHandler.JoystickUpped -= OnJoystickUpped;
        }


        private void OnJoystickUpdated(Vector2 obj)
        {
            var direction = new Vector3(obj.x, 0, obj.y);
            _character?.Move(direction);
        }

        private void OnJoystickUpped(Vector2 joystickValue)
        {
            _character?.Move(Vector3.zero);
        }

        private void OnJoystickDowned()
        {

        }
    }
}