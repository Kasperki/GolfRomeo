using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RaceManager : Singleton<RaceManager>
{
    public const int MaxPlayers = 8;
    public List<Player> Players;
    public StadingsCalculator StandingsCalculator;
    public StandingsUI StandingsUI;

    public List<string> TrackNames;
    public int CurrentTrack;
    public int Laps = 1;

    private bool raceEnded;

    private new void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (raceEnded)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                if (CurrentTrack == TrackNames.Count)
                {
                    FindObjectOfType<PlayUI>().Init();
                }
                else
                {
                    LoadNextRace();
                }
            }
        }
    }

    public void StartNewTournament()
    {
        StandingsCalculator = new StadingsCalculator(Players, StandingsUI);
    }

    public void EditTrack()
    {
        GameManager.SetState(State.Edit);
        Track.Instance.LoadTrack(TrackNames[CurrentTrack++]);
    }

    public void LoadNextRace()
    {
        GameManager.SetState(State.Pause);

        //Load World
        Track.Instance.LoadTrack(TrackNames[CurrentTrack++]);

        //Load Cars
        var cars = LoadCars();

        //Init Lap Tracker
        Track.Instance.LapTracker.Initialize(cars);

        StartCoroutine(StartCountDown());
    }

    private IEnumerator StartCountDown()
    {
        GameManager.SetState(State.Pause);
        var startTime = Time.time + 3.5f;

        while (Time.time < startTime)
        {
            FindObjectOfType<CountDown>().UpdateCountdown(startTime - Time.time - 0.5f);
            yield return null;
        }

        StartRace();
        yield return null;
    }

    private Car[] LoadCars()
    {
        Car[] cars = new Car[Players.Count];

        for (int i = 0; i < Players.Count; i++)
        {
            var obj = Instantiate(Resources.Load("Cars/" + Players[i].CarType.ToString())) as GameObject;
            cars[i] = obj.GetComponent<Car>();
            cars[i].Init(Players[i]);
        }

        return cars;
    }


    private void StartRace()
    {
        GameManager.SetState(State.Game);
        Track.Instance.LapTracker.StartTimer();
    }

    public void EndRace()
    {
        GameManager.SetState(State.Pause);
        raceEnded = true;
        StandingsCalculator.ShowStandings();

        if (CurrentTrack == TrackNames.Count)
        {
            StandingsCalculator.ShowWinners();
        }
    }
}
