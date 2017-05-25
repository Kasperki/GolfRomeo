using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static bool SubmitPressed()
    {
        if (Input.GetKeyDown(new ControllerScheme().Keyboard().Submit) || Input.GetKeyDown(new ControllerScheme().Joystick1().Submit))
        {
            return true;
        }

        return false;
    }

	public static bool BackPressed()
    {
        if (Input.GetKeyDown(new ControllerScheme().Keyboard().Select) || Input.GetKeyDown(new ControllerScheme().Joystick1().Select))
        {
            return true;
        }

        return false;
    }

    public static bool StartPressed()
    {
        if (Input.GetKeyDown(new ControllerScheme().Keyboard().Start) || Input.GetKeyDown(new ControllerScheme().Joystick1().Start))
        {
            return true;
        }

        return false;
    }
}
