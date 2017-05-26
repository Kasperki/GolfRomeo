using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CursorUI : CursorBaseUI
{
    public GameObject EditorObject;
    public CursorTerrainHeightEditorUI TerrainEditorUI;
    public CursorBrushUI BrushUI;
    public CursorObjectsUI CursorObjectsUI;

    public bool DriveTesting;
    public GameObject TestCar;

    private EditorUI editorUI;

    new void Awake()
    {
        base.Awake();
        editorUI = FindObjectOfType<EditorUI>();
    }

	new void Update ()
    {
        transform.eulerAngles = Vector3.zero;

        if (EditorObject.activeSelf == false)
        {
            return;
        }

        base.Update();
    }

    public void Init()
    {
        EditorObject.SetActive(true);
        Destroy(TestCar);
        DriveTesting = false;

        if (TerrainEditorUI.gameObject.activeSelf == false && BrushUI.gameObject.activeSelf == false && CursorObjectsUI.gameObject.activeSelf == false)
        {
            Open();
        }
    }

    public void Exit()
    {
        EditorObject.SetActive(false);
    }

    public override void Open()
    {
        ButtonsRect.gameObject.SetActive(true);
    }

    public override void Close()
    {
        if (TerrainEditorUI.gameObject.activeSelf == false && BrushUI.gameObject.activeSelf == false && CursorObjectsUI.gameObject.activeSelf == false)
        {
            Exit();
        }
        else
        {
            ButtonsRect.gameObject.SetActive(false);
        }
    }

    public void EditTerrain()
    {
        TerrainEditorUI.Open();
        Close();
    }

    public void EditTexture()
    {
        BrushUI.Open();
        Close();
    }

    public void EditObjects()
    {
        CursorObjectsUI.Open();
        Close();
    }

    public void TestMap()
    {
        Player player = new Player();
        player.ControllerScheme = new ControllerScheme()
        {
            HorizontalAxis = "Horizontal",
            VerticalAxis = "Vertical"
        };

        TestCar = ResourcesLoader.LoadCar(player.CarType);
        TestCar.GetComponent<Car>().Init(player);
        TestCar.transform.position = transform.position;
        DriveTesting = true;

        Exit();
    }

    public void Save()
    {
        editorUI.MapEditorMenuUI.Init();
    }

    public bool IsActive()
    {
        return EditorObject.activeSelf;
    }
}
