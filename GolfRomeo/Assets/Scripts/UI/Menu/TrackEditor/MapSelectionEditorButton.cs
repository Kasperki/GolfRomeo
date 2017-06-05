using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

class MapSelectionEditorButton : MonoBehaviour
{
    public Color selectedColor = Color.white;
    public Color normalColor = Color.white;

    public Image image;
    private Button button;
    private Text text;

    void Awake()
    {
        image = GetComponent<Image>();
        button = GetComponent<Button>();
        text = GetComponentInChildren<Text>();
    }

    public void SetListener(string track)
    {
        text.text = track;

        button.onClick.AddListener(() =>
        {
            foreach (var btn in FindObjectsOfType<MapSelectionEditorButton>())
            {
                btn.image.color = normalColor;
            }

            RaceManager.Instance.TrackNames.Clear();
            RaceManager.Instance.TrackNames.Add(track);
            image.color = selectedColor;
        });
    }
}
