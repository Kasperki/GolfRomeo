using UnityEngine;
using UnityEngine.UI;

public class QualitySettingsButton : MonoBehaviour
{
    public Color normalColor;
    public Color selectedColor;

    public Image image;
    private Button button;
    private Text text;

    void Awake()
    {
        image = GetComponent<Image>();
        button = GetComponent<Button>();
        text = GetComponentInChildren<Text>();
    }

    public void SetListener(string qualitySettingName, int qualitySettingLevel)
    {
        text.text = qualitySettingName;

        button.onClick.AddListener(() =>
        {
            foreach (var btn in FindObjectsOfType<QualitySettingsButton>())
            {
                btn.image.color = normalColor;
            }

            QualitySettings.SetQualityLevel(qualitySettingLevel);
            SetActive();
        });
    }

	public void SetActive()
    {
        image.color = selectedColor;
    }
}
