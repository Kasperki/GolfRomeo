using UnityEngine;

public class OptionsManager : Singleton<OptionsManager>
{
    private const float DefaultVolume = 1.0f;
    private int DefaultGraphicsLevel { get { return QualitySettings.names.Length; } }

    void Start ()
    {
        QualitySettings.SetQualityLevel(LoadGraphics());
        AudioListener.volume = LoadVolume();
    }
	
	public void SaveGraphics(int qualitySettingLevel)
    {
        PlayerPrefs.SetInt(Constants.Options_Graphics, qualitySettingLevel);
    }

    private int LoadGraphics()
    {
        if (PlayerPrefs.HasKey(Constants.Options_Graphics))
        {
            return PlayerPrefs.GetInt(Constants.Options_Graphics);
        }

        return DefaultGraphicsLevel;
    }

    public void SaveVolume(float volume)
    {
        PlayerPrefs.SetFloat(Constants.Options_Volume, AudioListener.volume);
    }

    private float LoadVolume()
    {
        if (PlayerPrefs.HasKey(Constants.Options_Volume))
        {
            return PlayerPrefs.GetFloat(Constants.Options_Volume);
        }

        return DefaultVolume;
    }
}
