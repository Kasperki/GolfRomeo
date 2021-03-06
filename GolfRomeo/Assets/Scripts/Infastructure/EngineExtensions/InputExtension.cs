﻿using UnityEngine;


public class InputExtension : MonoBehaviour
{
    private bool leftTrigger, rightTrigger;
    private bool left, right;

    public string horizontalAxis = "Horizontal";

    public void Update()
    {
        left = false;
        right = false;

        //Left trigger
        if (Input.GetAxisRaw(horizontalAxis) == -1 && leftTrigger == false)
        {
            left = true;
            leftTrigger = true;
        }

        if (Input.GetAxisRaw(horizontalAxis) >= 0 && leftTrigger == true)
        {
            leftTrigger = false;
        }

        //Right trigger
        if (Input.GetAxisRaw(horizontalAxis) == 1 && rightTrigger == false)
        {
            right = true;
            rightTrigger = true;
        }

        if (Input.GetAxisRaw(horizontalAxis) <= 0 && rightTrigger == true)
        {
            rightTrigger = false;
        }
    }

    public bool LeftTriggerDown()
    {
        return left;
    }

    public bool RightTriggerDown()
    {
        return right;
    }
}
