using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ControllerScheme
{
    //Select & Menu
    public KeyCode Select = KeyCode.Escape;
    public KeyCode Start = KeyCode.Return;

    //4 Buttons
    public KeyCode Submit = KeyCode.Space;
    public KeyCode Cancel = KeyCode.LeftControl;
    public KeyCode Duplicate = KeyCode.H;
    public KeyCode Delete = KeyCode.Delete;

    //Extra
    public KeyCode Rotate = KeyCode.R;

    //Axis
    public string VerticalAxis;
    public string HorizontalAxis;

    public ControllerScheme Keyboard()
    {
        return new ControllerScheme()
        {
            VerticalAxis = "Vertical",
            HorizontalAxis = "Horizontal",
            Select = KeyCode.Escape,
            Start = KeyCode.Return,
            Submit = KeyCode.Space,
            Cancel = KeyCode.LeftControl,
        };
    }

    public ControllerScheme Keyboard2()
    {
        return new ControllerScheme()
        {
            VerticalAxis = "VerticalDebug",
            HorizontalAxis = "HorizontalDebug",
            Start = KeyCode.K,
            Submit = KeyCode.I,
            Cancel = KeyCode.J,
        };
    }

    public ControllerScheme Joystick1()
    {
        return new ControllerScheme()
        {
            VerticalAxis = "Vertical1",
            HorizontalAxis = "Horizontal1",
            Start = KeyCode.Joystick1Button7,
            Submit = KeyCode.Joystick1Button0,
            Cancel = KeyCode.Joystick1Button1,
        };
    }
}
