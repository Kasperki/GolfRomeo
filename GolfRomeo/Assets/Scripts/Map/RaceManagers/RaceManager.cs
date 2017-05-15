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
            if (Input.GetKeyDown(KeyCode.Space))
            {
                StandingsCalculator.HideStandings();

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
        CurrentTrack = 0;
        StandingsCalculator = new StadingsCalculator(Players, StandingsUI);
        LoadNextRace();
    }

    public void EditTrack()
    {
        GameManager.SetState(State.Edit);
        Track.Instance.LoadTrack(TrackNames[0]);
    }

    public void LoadNextRace()
    {
        GameManager.SetState(State.Pause);

        //Clean up.
        raceEnded = false;

        var oldCars = FindObjectsOfType<Car>();
        for (int i = 0; i < oldCars.Length; i++)
        {
            Destroy(oldCars[i].gameObject);
        }

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
        FindObjectOfType<CountDown>().Awake();
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
            var obj = ResourcesLoader.LoadCar(Players[i].CarType);
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

        StandingsCalculator.UpdateStandings(Track.Instance.LapTracker.Cars);
        StandingsCalculator.ShowStandings();

        if (CurrentTrack == TrackNames.Count)
        {
            StandingsCalculator.ShowWinners();
        }
    }
}
