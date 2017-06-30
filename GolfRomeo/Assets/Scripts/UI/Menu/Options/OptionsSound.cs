using UnityEngine.UI;
using UnityEngine;

public class OptionsSound : MonoBehaviour
{
    public Text Volume;

    void Start()
    {
        SaveAndUpdateVolumeUI();
    }

	public void DecreaseVolume()
    {
        AudioListener.volume -= 0.01f;
        SaveAndUpdateVolumeUI();
    }

    public void IncreaseVolue()
    {
        AudioListener.volume += 0.01f;
        SaveAndUpdateVolumeUI();
    }

    public void SaveAndUpdateVolumeUI()
    {
        OptionsManager.Instance.SaveVolume(AudioListener.volume);

        AudioListener.volume = Mathf.Clamp(AudioListener.volume, 0, 1);
        Volume.text = AudioListener.volume.ToString("0.00");
    }
}
