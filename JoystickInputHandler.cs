using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickInputHandler : MonoBehaviour
{
    [SerializeField]
    private Joystick _joystick;

    public event Action JoystickDowned;
    public event Action<Vector2> JoystickUpped;
    public event Action<Vector2> JoystickUpdated;

    private bool _joystickIsActive = false;

    void OnEnable()
    {
        _joystick.PointerUpped += OnPointerUpped;
        _joystick.PointerDowned += OnPointerDowned;
    }
    void OnDisable()
    {
        _joystick.PointerUpped -= OnPointerUpped;
        _joystick.PointerDowned -= OnPointerDowned;
    }

    private void OnPointerUpped(Joystick obj)
    {
        var joystickValue = new Vector2(obj.Horizontal, obj.Vertical);

        JoystickUpped?.Invoke(joystickValue);
        _joystickIsActive = false;
    }
    private void OnPointerDowned(Joystick obj)
    {
        _joystickIsActive = true;
        JoystickDowned?.Invoke();
    }

    void FixedUpdate()
    {
        if (!_joystickIsActive)
            return;

        var vector = new Vector2(_joystick.Horizontal, _joystick.Vertical);
        JoystickUpdated?.Invoke(vector);
    }
}
