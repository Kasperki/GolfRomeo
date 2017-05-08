using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDown : MonoBehaviour
{
    public Text text3, text2, text1, textgo;
    public Vector3 text3Start, text2Start, text1Start, textgoStart;

	void Awake ()
    {
        text3Start = text3.transform.rotation.eulerAngles;
        text2Start = text2.transform.rotation.eulerAngles;
        text1Start = text1.transform.rotation.eulerAngles;
        textgoStart = textgo.transform.rotation.eulerAngles;
    }
	
    public void UpdateCountdown(float TimeUntilStart)
    {
        StartCoroutine(FlyText(text3.gameObject, text3Start));
    }

    IEnumerator FlyText(GameObject obj, Vector3 rotation)
    {
        obj.SetActive(true);
        float startTime = Time.time;
        float t = 0;

        while (t < 1)
        {
            t = Time.time - startTime;
            obj.transform.eulerAngles = Vector3.Lerp(rotation, Vector3.zero, t);
            yield return null;
        }

        obj.SetActive(false);
        yield return null;
    }
}
