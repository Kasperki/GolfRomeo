using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class CursorTrackObjectsUI : CursorBaseUI
{
    private CursorUI CursorEditorUI;

    public new void Start()
    {
        base.Start();

        CursorEditorUI = GetComponentInParent<CursorUI>();
        var trackObjects = Resources.LoadAll(ResourcesLoader.TRACKOBJECTS, typeof(GameObject)).Cast<GameObject>();

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

            Texture2D prev = AssetPreview.GetAssetPreview(obj.gameObject);
            if (prev != null)
            {
                var sprite = Sprite.Create(prev, new Rect(0, 0, prev.width, prev.height), Vector2.zero);
                image.sprite = sprite;
            }

            button.onClick.AddListener(() =>  {
                CursorEditor.CreateNewObject(ResourcesLoader.LoadTrackObject(trackObj.ID));
                ButtonsRect.gameObject.SetActive(false);
                CursorEditorUI.Exit();
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
