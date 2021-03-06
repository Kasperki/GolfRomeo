﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;

/// <summary>
/// Based on: http://answers.unity3d.com/answers/1227320/view.html
/// </summary>
public class HoldButton : Button
{
    // Event delegate triggered on mouse or touch down.
    [SerializeField]
    private ButtonDownEvent onDown = new ButtonDownEvent();

    private bool onHold = false;
    private bool listenForSubmitUp;

    protected HoldButton() { }

    public override void OnSubmit(BaseEventData eventData)
    {
        base.OnSubmit(eventData);
        onHold = true;
        listenForSubmitUp = true;
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        onHold = true;
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
        onHold = false;
    }

    private void Update()
    {
        if (onHold)
        {
            onDown.Invoke();
        }

        if (listenForSubmitUp)
        {
            if (Input.GetKeyUp(KeyCode.Space)) //TODO THIS SHOULD BE GLOBAL EVENT SYSTEM SUBMIT BUTTON
            {
                listenForSubmitUp = false;
                onHold = false;
            }
        }
    }

    public ButtonDownEvent OnHold
    {
        get { return onDown; }
        set { onDown = value; }
    }

    [Serializable]
    public class ButtonDownEvent : UnityEvent { }
}

