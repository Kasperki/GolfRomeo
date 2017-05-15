using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ControllerScheme
{
    public string VerticalAxis;
    public string HorizontalAxis;

    public KeyCode Menu = KeyCode.Return;
    public KeyCode Select = KeyCode.Space;
    public KeyCode Cancel = KeyCode.Escape;

    public KeyCode Duplicate = KeyCode.D;
    public KeyCode Delete = KeyCode.Delete;
    public KeyCode Rotate = KeyCode.R;

    public ControllerScheme Keyboard()
    {
        return new ControllerScheme()
        {
            VerticalAxis = "Vertical",
            HorizontalAxis = "Horizontal",
            Menu = KeyCode.Return,
            Select = KeyCode.Space,
            Cancel = KeyCode.Escape,
        };
    }

    public ControllerScheme Keyboard2()
    {
        return new ControllerScheme()
        {
            VerticalAxis = "VerticalDebug",
            HorizontalAxis = "HorizontalDebug",
            Menu = KeyCode.K,
            Select = KeyCode.I,
            Cancel = KeyCode.J,
        };
    }

    public ControllerScheme Joystick1()
    {
        return new ControllerScheme()
        {
            VerticalAxis = "Vertical1",
            HorizontalAxis = "Horizontal1",
            Menu = KeyCode.Joystick1Button0,
            Select = KeyCode.Joystick1Button1,
            Cancel = KeyCode.Joystick1Button2,
        };
    }
}
