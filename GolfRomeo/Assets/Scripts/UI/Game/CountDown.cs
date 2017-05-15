using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDown : MonoBehaviour
{
    public Text text3, text2, text1, textgo;
    public Vector3 text3Start, text2Start, text1Start, textgoStart;
    private bool text3animating, text2animating, text1animating, textgoanimating;

    public void Awake ()
    {
        text3animating = false;
        text2animating = false;
        text1animating = false;
        textgoanimating = false;

        text3.transform.rotation = Quaternion.Euler(Random.Range(-80, -110), Random.Range(-270, 270), Random.Range(-270, 270));
        text2.transform.rotation = Quaternion.Euler(Random.Range(-270, 270), Random.Range(-80, -110), Random.Range(-270, 270));
        text1.transform.rotation = Quaternion.Euler(Random.Range(-170, 170), Random.Range(-80, -110), Random.Range(-10, 20));
        textgo.transform.rotation = Quaternion.Euler(Random.Range(-370, 370), Random.Range(-200, 210), Random.Range(-50, 50));
    }
	
    public void UpdateCountdown(float TimeUntilStart)
    {
        if (TimeUntilStart <= 3 && text3animating == false)
        {
            text3animating = true;
            StartCoroutine(FlyText(text3.gameObject, Quaternion.Euler(text3Start.x, text3Start.y, text3Start.z)));
        }
        else if (TimeUntilStart <= 2 && text2animating == false)
        {
            text2animating = true;
            StartCoroutine(FlyText(text2.gameObject, Quaternion.Euler(text2Start.x, text2Start.y, text2Start.z)));
        }
        else if (TimeUntilStart <= 1 && text1animating == false)
        {
            text1animating = true;
            StartCoroutine(FlyText(text1.gameObject, Quaternion.Euler(text1Start.x, text1Start.y, text1Start.z)));
        }
        else if (TimeUntilStart <= 0 && textgoanimating == false)
        {
            textgoanimating = true;
            StartCoroutine(FlyText(textgo.gameObject, Quaternion.Euler(textgoStart.x, textgoStart.y, textgoStart.z)));
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
            obj.transform.rotation = Quaternion.Slerp(rotation, Quaternion.identity, Mathf.SmoothStep(0,4,t));
            yield return null;
        }

        obj.SetActive(false);
        yield return null;
    }
}
