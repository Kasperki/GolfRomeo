using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RaceManager : Singleton<RaceManager>
{
    public List<Player> Players;

    public List<string> TrackNames;
    public int CurrentTrack;
    public int Laps;

    //OTHER INFO
    //TODO CALCULATE STADINGS

    private new void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }

    void Start ()
    {
        TrackNames = new List<string>() { "ThisIsATest", "Canovol" };

        Players = new List<Player>();

        var player1 = new Player();
        player1.Name = "Pelaaja 1";
        player1.PrimaryColor = Color.black;
        player1.SecondaryColor = Color.white;
        player1.CarType = CarType.Formula;

        var player2 = new Player();
        player2.Name = "Player 2";
        player2.PrimaryColor = Color.red;
        player2.SecondaryColor = Color.magenta;
        player1.CarType = CarType.Formula;

        var player3 = new Player();
        player3.Name = "AI 3";
        player3.PrimaryColor = Color.red;
        player3.SecondaryColor = Color.magenta;
        player3.PlayerType = PlayerType.AI;
        player1.CarType = CarType.Truck;

        Players.Add(player1);
        Players.Add(player2);
        Players.Add(player3);
    }
    

    public void LoadNextRace()
    {
        //TODO CLEAN UP

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
