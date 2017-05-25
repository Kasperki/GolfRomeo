using UnityEngine;

public class PauseUI : MonoBehaviour
{
    public PlayUI PlayUI;
    public LapTracker LapTracker;

    public RectTransform Parent;

    private float pausedTime;
    Vector3[] cachedCarVelocity;

    public void Update()
    {
        if (InputManager.BackPressed())
        {
            if (GameManager.CheckState(State.Game))
            {
                Pause();
            }
        }
    }

    public void Pause()
    {
        Parent.gameObject.SetActive(true);
        GameManager.SetState(State.Pause);
        pausedTime = Time.time;
        Time.timeScale = 0;
    }

    public void Continue()
    {
        Parent.gameObject.SetActive(false);
        float timePaused = Time.time - pausedTime;
        GameManager.SetState(State.Game);
        Time.timeScale = 1;
    }

    public void Exit()
    {
        Time.timeScale = 1;
        Track.Instance.LapTracker.LapTrackerUI.Hide();

        Parent.gameObject.SetActive(false);
        GameManager.SetState(State.Menu);
        PlayUI.Init();
    }
}
