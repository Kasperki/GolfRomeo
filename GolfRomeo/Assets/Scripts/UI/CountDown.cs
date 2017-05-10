using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDown : MonoBehaviour
{
    public Text text3, text2, text1, textgo;
    private Quaternion text3Start, text2Start, text1Start, textgoStart;
    private bool text3animating, text2animating, text1animating, textgoanimating;

    void Awake ()
    {
        text3animating = false;
        text2animating = false;
        text1animating = false;
        textgoanimating = false;

        text3Start = text3.transform.rotation;
        text2Start = text2.transform.rotation;
        text1Start = text1.transform.rotation;
        textgoStart = textgo.transform.rotation;
    }
	
    public void UpdateCountdown(float TimeUntilStart)
    {
        if (TimeUntilStart <= 3 && text3animating == false)
        {
            text3animating = true;
            StartCoroutine(FlyText(text3.gameObject, text3Start));
        }
        else if (TimeUntilStart <= 2 && text2animating == false)
        {
            text2animating = true;
            StartCoroutine(FlyText(text2.gameObject, text2Start));
        }
        else if (TimeUntilStart <= 1 && text1animating == false)
        {
            text1animating = true;
            StartCoroutine(FlyText(text1.gameObject, text1Start));
        }
        else if (TimeUntilStart <= 0 && textgoanimating == false)
        {
            textgoanimating = true;
            StartCoroutine(FlyText(textgo.gameObject, textgoStart));
        }
    }

    IEnumerator FlyText(GameObject obj, Quaternion rotation)
    {
        obj.SetActive(true);
        float startTime = Time.time;
        float t = 0;

        while (t < 1)
        {
            t = Time.time - startTime;
            obj.transform.rotation = Quaternion.Slerp(rotation, Quaternion.identity, t);
            yield return null;
        }

        obj.SetActive(false);
        yield return null;
    }
}
