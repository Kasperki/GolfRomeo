using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapButton : MonoBehaviour
{
    public Color selectedColor, normalColor;

    private bool selected;
    private Button button;
    private Image image;
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
            selected = !selected;

            if (selected)
            {
                RaceManager.Instance.TrackNames.Add(track);
                image.color = selectedColor;
            }
            else
            {
                RaceManager.Instance.TrackNames.Remove(track);
                image.color = normalColor;
            }
        });
    }
}
