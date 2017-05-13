using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CursorUI : CursorBaseUI
{
    public GameObject EditorObject;
    public CursorTerrainHeightEditorUI TerrainEditorUI;
    public CursorTerrainTextureEditorUI TerrainTextureEditorUI;
    public CursorTrackObjectsUI TrackObjectsUI;

    private GameObject testCar;
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
        Destroy(testCar);

        if (TerrainEditorUI.gameObject.activeSelf == false && TerrainTextureEditorUI.gameObject.activeSelf == false && TrackObjectsUI.gameObject.activeSelf == false)
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
        if (TerrainEditorUI.gameObject.activeSelf == false && TerrainTextureEditorUI.gameObject.activeSelf == false && TrackObjectsUI.gameObject.activeSelf == false)
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
        TerrainTextureEditorUI.Open();
        Close();
    }

    public void EditObjects()
    {
        TrackObjectsUI.Open();
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

        testCar = ResourcesLoader.LoadCar(player.CarType);
        testCar.GetComponent<Car>().Init(player);
        testCar.transform.position = transform.position;

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
