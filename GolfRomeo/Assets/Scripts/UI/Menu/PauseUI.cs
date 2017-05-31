using UnityEngine;

public class PauseUI : MonoBehaviour
{
    public PlayUI PlayUI;
    public LapTracker LapTracker;
    public RectTransform Parent;

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

        Time.timeScale = 0;
    }

    public void Continue()
    {
        Parent.gameObject.SetActive(false);
        GameManager.SetState(State.Game);

        Time.timeScale = 1;
    }

    public void NextRace()
    {
        Parent.gameObject.SetActive(false);
        RaceManager.Instance.LoadNextRace();

        Time.timeScale = 1;
    }

    public void Exit()
    {
        Parent.gameObject.SetActive(false);
        RaceManager.Instance.EndRace();

        Time.timeScale = 1;
    }
}
