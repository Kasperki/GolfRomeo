using UnityEngine.UI;
using UnityEngine;

public class OptionsSound : MonoBehaviour
{
    public Text Volume;

	public void DecreaseVolume()
    {
        AudioListener.volume -= 0.01f;
        UpdateVolumeText();
    }

    public void IncreaseVolue()
    {
        AudioListener.volume += 0.01f;
        UpdateVolumeText();
    }

    public void UpdateVolumeText()
    {
        AudioListener.volume = Mathf.Clamp(AudioListener.volume, 0, 1);
        Volume.text = AudioListener.volume.ToString("0.00");
    }
}
