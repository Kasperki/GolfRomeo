using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CursorUI : MonoBehaviour
{
    public CursorEditor CursorEditor;
    public GameObject UIParent;
    public RectTransform ButtonsRect;
    public List<Button> Buttons;

    public TerrainEditorUI TerrainEditorUI;

    private int selection;
    private InputManagerExtension inputManager;

	void Start ()
    {
        inputManager = gameObject.AddComponent<InputManagerExtension>();
	}
	
	void Update ()
    {
        transform.eulerAngles = Vector3.zero;

        if (Input.GetKeyDown(KeyCode.Return))
        {
            UIParent.SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            UIParent.SetActive(false);
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

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Buttons[selection].onClick.Invoke();
        }
    }

    public void EditTerrain()
    {
        TerrainEditorUI.Open();
        Close();
    }

    public void EditObjects()
    {
        CursorEditor.EditMode = EditMode.Objects;
    }

    public void TestMap()
    {
        //SPAWN CAR WITH THIS CURSOR CONTROL SCHEME
    }

    public void Save()
    {
        //OPEN SAVE DIALOG, SAVE OR DISCARD
    }

    public void Open()
    {
        ButtonsRect.gameObject.SetActive(true);
    }

    public void Close()
    {
        ButtonsRect.gameObject.SetActive(false);
    }
}
