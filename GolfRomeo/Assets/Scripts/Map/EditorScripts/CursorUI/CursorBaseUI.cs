using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class CursorBaseUI : MonoBehaviour
{
    protected CursorEditor CursorEditor;
    protected RectTransform buttonsRect { get { return GetComponent<RectTransform>(); } }
    protected List<Button> buttons;

    private int selection;
    private InputExtension inputManager;

    protected void Awake()
    {
        CursorEditor = GetComponentInParent<CursorEditor>();
        buttons = new List<Button>(GetComponentsInChildren<Button>());
    }

    protected void Start()
    {
        inputManager = gameObject.AddComponent<InputExtension>();
        inputManager.horizontalAxis = CursorEditor.ControlScheme.HorizontalAxis;
    }

    protected void Update()
    {
        if (buttonsRect.gameObject.activeSelf == false)
        {
            return;
        }

        if (Input.GetKeyUp(CursorEditor.ControlScheme.Cancel))
        {
            Close();
        }

        if (inputManager.LeftTriggerDown() && selection > 0)
        {
            buttonsRect.position += new Vector3(2.5f, 0);
            selection--;
        }
        else if (inputManager.RightTriggerDown() && selection < buttons.Count - 1)
        {
            buttonsRect.position -= new Vector3(2.5f, 0);
            selection++;
        }

        if (Input.GetKeyUp(CursorEditor.ControlScheme.Submit))
        {
            buttons[selection].onClick.Invoke();
        }
    }

    public abstract void Open();

    public abstract void Close();
}
