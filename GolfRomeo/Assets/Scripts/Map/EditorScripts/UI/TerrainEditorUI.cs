using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TerrainEditorUI : MonoBehaviour
{
    public CursorEditor CursorEditor;
    public GameObject UIParent;
    public RectTransform ButtonsRect;
    public List<Button> Buttons;

    public void Open()
    {
        CursorEditor.EditMode = EditMode.Terrain;
        ButtonsRect.gameObject.SetActive(true);
    }

    public void Close()
    {
        ButtonsRect.gameObject.SetActive(false);
        
    }
}
