using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsVideo : MonoBehaviour
{
    public Transform QualitySettingsParent;
    public GameObject QualitySettingsButton;

	void Start ()
    {
        var qualitySettings = QualitySettings.names;

        for (int i = 0; i < qualitySettings.Length; i++)
        {
            var obj = Instantiate(QualitySettingsButton);
            obj.transform.SetParent(QualitySettingsParent, false);
            obj.GetComponent<QualitySettingsButton>().SetListener(qualitySettings[i], i);

            if (i == QualitySettings.GetQualityLevel())
            {
                obj.GetComponent<QualitySettingsButton>().SetActive();
            }
        }
	}
}
