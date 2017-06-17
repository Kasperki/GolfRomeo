using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class InputManager : Singleton<InputManager>
{
    private List<ControllerScheme> avaiableControllers;

    private void Start()
    {
        avaiableControllers = new List<ControllerScheme>();

        int controllerCount = Input.GetJoystickNames().Length;

        if (controllerCount > 0)
        {
            avaiableControllers.Add(new ControllerScheme().Joystick1());
        }
        if (controllerCount > 1)
        {
            avaiableControllers.Add(new ControllerScheme().Joystick2());
        }
        if (controllerCount > 2)
        {
            avaiableControllers.Add(new ControllerScheme().Joystick3());
        }
        if (controllerCount > 3)
        {
            avaiableControllers.Add(new ControllerScheme().Joystick4());
        }
        if (controllerCount > 4)
        {
            avaiableControllers.Add(new ControllerScheme().Joystick5());
        }
        if (controllerCount > 5)
        {
            avaiableControllers.Add(new ControllerScheme().Joystick6());
        }
        if (controllerCount > 6)
        {
            avaiableControllers.Add(new ControllerScheme().Joystick7());
        }
        if (controllerCount > 7)
        {
            avaiableControllers.Add(new ControllerScheme().Joystick8());
        }

        avaiableControllers.Add(new ControllerScheme().Keyboard());
        avaiableControllers.Add(new ControllerScheme().Keyboard2());
        avaiableControllers.Add(new ControllerScheme().Keyboard3());
        avaiableControllers.Add(new ControllerScheme().Keyboard4());
    }

    public ReadOnlyCollection<ControllerScheme> GetAvailableControllerSchemes()
    {
        return avaiableControllers.AsReadOnly();
    }

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
