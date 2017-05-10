using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CursorUI : CursorEditorUI
{
    public GameObject EditorObject;
    public TerrainEditorUI TerrainEditorUI;
    public TerrainTextureEditorUI TerrainTextureEditorUI;
    public TrackObjectsUI TrackObjectsUI;

    private GameObject testCar;

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

        testCar = Instantiate(Resources.Load("Cars/" + player.CarType.ToString())) as GameObject;
        testCar.GetComponent<Car>().Init(player);
        testCar.transform.position = transform.position;

        Exit();
    }

    public void Save()
    {
        Exit();
        //OPEN SAVE DIALOG, SAVE OR DISCARD
    }
}
