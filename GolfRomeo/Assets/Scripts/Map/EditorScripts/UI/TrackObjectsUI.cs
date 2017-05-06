using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TrackObjectsUI : EditorUI
{
    private CursorUI CursorEditorUI;

    public void Start()
    {
        CursorEditorUI = GetComponentInParent<CursorUI>();
        var trackObjects = Resources.LoadAll("Objects", typeof(GameObject)).Cast<GameObject>();

        Buttons = new List<Button>();

        foreach (var obj in trackObjects)
        {
            var trackObj = obj.GetComponent<TrackObject>();

            var gameObj = new GameObject();
            gameObj.transform.SetParent(transform, false);
            gameObj.AddComponent<RectTransform>();
            gameObj.GetComponent<RectTransform>().sizeDelta = new Vector2(10, 10);

            var button = gameObj.AddComponent<Button>();
            var image = gameObj.AddComponent<Image>();
            button.targetGraphic = image;
            //image.sprite = obj.icon;

            button.onClick.AddListener(() =>  {
                CursorEditor.CheckpointPrefab = Resources.Load("Objects/" + trackObj.ID) as GameObject;

            });

            Buttons.Add(button);
        }
    }

    public override void Close()
    {
        ButtonsRect.gameObject.SetActive(false);
        CursorEditorUI.Open();
    }

    public override void Open()
    {
        ButtonsRect.gameObject.SetActive(true);
        CursorEditor.EditMode = EditMode.Objects;
    }
}
