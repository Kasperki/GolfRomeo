using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class CursorTrackObjectsUI : CursorBaseUI
{
    public CursorUI CursorEditorUI;

    public new void Start()
    {
        base.Start();

        var trackObjects = Resources.LoadAll(ResourcesLoader.Track_Objects, typeof(GameObject)).Cast<GameObject>();

        buttons = new List<Button>();

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
                buttonsRect.gameObject.SetActive(false);
                CursorEditorUI.Exit();
            });

            buttons.Add(button);
        }
    }

    public override void Close()
    {
        buttonsRect.gameObject.SetActive(false);
        CursorEditorUI.Open();
    }

    public override void Open()
    {
        buttonsRect.gameObject.SetActive(true);
        CursorEditor.EditMode = EditMode.Objects;
    }
}
