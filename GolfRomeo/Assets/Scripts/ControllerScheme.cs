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
}
