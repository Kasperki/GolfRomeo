using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class CursorBaseUI : MonoBehaviour
{
    protected CursorEditor CursorEditor;
    public RectTransform ButtonsRect;
    public List<Button> Buttons;

    private int selection;
    private InputManagerExtension inputManager;

    protected void Awake()
    {
        CursorEditor = GetComponentInParent<CursorEditor>();
        inputManager = gameObject.AddComponent<InputManagerExtension>();
    }

    protected void Update()
    {
        if (ButtonsRect.gameObject.activeSelf == false)
        {
            return;
        }

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Close();
        }

        if (inputManager.LeftTriggerDown() && selection > 0)
        {
            ButtonsRect.position += new Vector3(2.5f, 0);
            selection--;
        }
        else if (inputManager.RightTriggerDown() && selection < Buttons.Count - 1)
        {
            ButtonsRect.position -= new Vector3(2.5f, 0);
            selection++;
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            Buttons[selection].onClick.Invoke();
        }
    }

    public abstract void Open();

    public abstract void Close();
}
