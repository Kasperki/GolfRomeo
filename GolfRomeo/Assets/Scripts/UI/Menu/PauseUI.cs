using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseUI : MonoBehaviour
{
    public PlayUI PlayUI;
    public LapTracker LapTracker;

    private float pausedTime;
    Vector3[] cachedCarVelocity;

    public void Pause()
    {
        GameManager.SetState(State.Pause);
        pausedTime = Time.time;

        var cars = FindObjectsOfType<CarController>();
        cachedCarVelocity = new Vector3[cars.Length];
        for (int i = 0; i < cars.Length; i++)
        {
            cachedCarVelocity[i] = cars[i].GetComponent<Rigidbody>().velocity;
        }
    }

    public void Continue()
    {
        float timePaused = Time.time - pausedTime;
        GameManager.SetState(State.Game);

        var cars = FindObjectsOfType<CarController>();
        cachedCarVelocity = new Vector3[cars.Length];
        for (int i = 0; i < cars.Length; i++)
        {
            cars[i].GetComponent<Rigidbody>().velocity = cachedCarVelocity[i];
        }
    }

    public void Exit()
    {
        GameManager.SetState(State.Menu);
        PlayUI.Init();
    }
}
