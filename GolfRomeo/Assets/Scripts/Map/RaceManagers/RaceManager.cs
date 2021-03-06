﻿using System.Collections.Generic;
using UnityEngine;

public class RaceManager : Singleton<RaceManager>
{
    public const int MaxPlayers = 8;
    public List<Player> Players;
    public StadingsCalculator StandingsCalculator;
    public StandingsUI StandingsUI;
    public RaceOptions RaceOptions;

    public List<string> TrackNames;
    public int CurrentTrack;

    private bool raceEnded;

    private new void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);

        RaceOptions = RaceOptions.DefaultRaceOptions;
    }

    private void Update()
    {
        if (raceEnded)
        {
            if (InputManager.SubmitPressed())
            {
                CleanUp();
                StandingsCalculator.HideStandings();

                if (CurrentTrack == TrackNames.Count)
                {
                    ReturnToMenu();
                }
                else
                {
                    LoadNextRace();
                }
            }
        }
    }

    public void InitializeSettings()
    {
        TrackNames = new List<string>();
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
        new TrackLoader(Track.Instance).LoadTrack(TrackNames[0]);
    }

    public void LoadNextRace()
    {
        FindObjectOfType<CameraZoom>().ResetCamera();

        if (CurrentTrack == TrackNames.Count)
        {
            EndRace();
        }
        else
        {
            CleanUp();

            //Load World
            new TrackLoader(Track.Instance).LoadTrack(TrackNames[CurrentTrack++]);

            var cars = LoadCars();

            //Start Race
            Track.Instance.LapTracker.Initialize(cars);
            WeatherManager.Instance.Initialize(cars);
        }
    }

    private void CleanUp()
    {
        raceEnded = false;

        foreach (var car in FindObjectsOfType<Car>())
        {
            Destroy(car.gameObject);
        }
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

    public void EndTournament()
    {
        CurrentTrack = TrackNames.Count;
        EndRace();
    }

    public void EndRace()
    {
        GameManager.SetState(State.Pause);
        raceEnded = true;

        StandingsCalculator.UpdateStandings(Track.Instance.LapTracker.CarOrder);
        Track.Instance.LapTracker.LapTrackerUI.Hide();

        if (CurrentTrack == TrackNames.Count)
        {
            StandingsCalculator.ShowWinners();
        }
        else
        {
            StandingsCalculator.ShowStandings();
        }
    }

    public void ReturnToMenu()
    {
        FindObjectOfType<PlayUI>().Init();
    }
}
