using System;
using UnityEngine;


/*
    TODO ---
    Keyboard custom mapping.
    - Default mappings to have all buttons.
*/

[Serializable]
public class ControllerScheme
{
    public string Name;

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
            Name = "WASD",
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
            Name = "ARROWS",
            VerticalAxis = "Keyboard2Vertical",
            HorizontalAxis = "Keyboard2Horizontal",
            Start = KeyCode.Period,
            Submit = KeyCode.RightControl,
            Cancel = KeyCode.Underscore,
        };
    }

    public ControllerScheme Keyboard3()
    {
        return new ControllerScheme()
        {
            Name = "IJKL",
            VerticalAxis = "Keyboard3Vertical",
            HorizontalAxis = "Keyboard3Horizontal",
            Start = KeyCode.B,
            Submit = KeyCode.M,
            Cancel = KeyCode.N,
        };
    }

    public ControllerScheme Keyboard4()
    {
        return new ControllerScheme()
        {
            Name = "Keypad 8456",
            VerticalAxis = "Keyboard4Vertical",
            HorizontalAxis = "Keyboard4Horizontal",
            Start = KeyCode.KeypadMinus,
            Submit = KeyCode.KeypadEnter,
            Cancel = KeyCode.Keypad0,
        };
    }

    public ControllerScheme Joystick1()
    {
        return new ControllerScheme()
        {
            Name = "JOYSTICK 1",
            VerticalAxis = "Vertical1",
            HorizontalAxis = "Horizontal1",
            Start = KeyCode.Joystick1Button7,
            Submit = KeyCode.Joystick1Button0,
            Cancel = KeyCode.Joystick1Button1,
        };
    }

    public ControllerScheme Joystick2()
    {
        return new ControllerScheme()
        {
            Name = "JOYSTICK 2",
            VerticalAxis = "Vertical2",
            HorizontalAxis = "Horizontal2",
            Start = KeyCode.Joystick2Button7,
            Submit = KeyCode.Joystick2Button0,
            Cancel = KeyCode.Joystick2Button1,
        };
    }

    public ControllerScheme Joystick3()
    {
        return new ControllerScheme()
        {
            Name = "JOYSTICK 3",
            VerticalAxis = "Vertical3",
            HorizontalAxis = "Horizontal3",
            Start = KeyCode.Joystick3Button7,
            Submit = KeyCode.Joystick3Button0,
            Cancel = KeyCode.Joystick3Button1,
        };
    }

    public ControllerScheme Joystick4()
    {
        return new ControllerScheme()
        {
            Name = "JOYSTICK 4",
            VerticalAxis = "Vertical4",
            HorizontalAxis = "Horizontal4",
            Start = KeyCode.Joystick4Button7,
            Submit = KeyCode.Joystick4Button0,
            Cancel = KeyCode.Joystick4Button1,
        };
    }

    public ControllerScheme Joystick5()
    {
        return new ControllerScheme()
        {
            Name = "JOYSTICK 5",
            VerticalAxis = "Vertical5",
            HorizontalAxis = "Horizontal5",
            Start = KeyCode.Joystick5Button7,
            Submit = KeyCode.Joystick5Button0,
            Cancel = KeyCode.Joystick5Button1,
        };
    }

    public ControllerScheme Joystick6()
    {
        return new ControllerScheme()
        {
            Name = "JOYSTICK 6",
            VerticalAxis = "Vertical6",
            HorizontalAxis = "Horizontal6",
            Start = KeyCode.Joystick6Button7,
            Submit = KeyCode.Joystick6Button0,
            Cancel = KeyCode.Joystick6Button1,
        };
    }

    public ControllerScheme Joystick7()
    {
        return new ControllerScheme()
        {
            Name = "JOYSTICK 7",
            VerticalAxis = "Vertical7",
            HorizontalAxis = "Horizontal7",
            Start = KeyCode.Joystick7Button7,
            Submit = KeyCode.Joystick7Button0,
            Cancel = KeyCode.Joystick7Button1,
        };
    }

    public ControllerScheme Joystick8()
    {
        return new ControllerScheme()
        {
            Name = "JOYSTICK 8",
            VerticalAxis = "Vertical8",
            HorizontalAxis = "Horizontal8",
            Start = KeyCode.Joystick8Button7,
            Submit = KeyCode.Joystick8Button0,
            Cancel = KeyCode.Joystick8Button1,
        };
    }
}
