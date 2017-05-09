﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RaceManager : Singleton<RaceManager>
{
    public const int MaxPlayers = 8;
    public List<Player> Players;
    public StadingsCalculator StandingsCalculator;

    public List<string> TrackNames;
    public int CurrentTrack;
    public int Laps = 1;

    //OTHER INFO
    //TODO CALCULATE STADINGS

    private new void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            LoadNextRace();
        }
    }

    public void LoadNextRace()
    {
        //TODO CLEAN UP
        StandingsCalculator = new StadingsCalculator(Players);

        //Load World
        Track.Instance.LoadWorld(TrackNames[CurrentTrack++]);

        //Load Cars
        LoadCars();

        //Init Lap Tracker
        Track.Instance.LapTracker.StartRace();
    }

    public void LoadCars()
    {
        foreach (var player in Players)
        {
            var obj = Instantiate(Resources.Load("Cars/" + player.CarType.ToString())) as GameObject;
            obj.GetComponent<Car>().Init(player);
        }
    }
}
