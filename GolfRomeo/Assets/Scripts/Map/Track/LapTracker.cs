using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LapTracker : Singleton<LapTracker>
{
    public int Laps;
    public Checkpoint[] Checkpoints { get { return GetComponentsInChildren<Checkpoint>(); } }
    public List<LapInfo> Cars;

    private float raceStartTime;

    public float LenghtInKilometers
    {
        get
        {
            float lenght = 0;

            for (int i = 0; i < Checkpoints.Length; i++)
            {
                int nextNode = i + 1 == Checkpoints.Length ? 0 : i + 1;
                lenght = (Checkpoints[i].transform.position - Checkpoints[nextNode].transform.position).magnitude;
            }

            return lenght;
        }
    }

    public int GetCarPosition(Guid ID)
    {
        return Cars.FindIndex(x => x.car.Player.ID == ID);
    }

    void Start()
    {
        Cars = new List<LapInfo>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            StartRace();
        }

        Cars = Cars.OrderByDescending(x => x.CurrentLap)
        .ThenByDescending(x => x.CurrentCheckpointID)
        .ThenBy(x => (x.car.transform.position - Checkpoints[x.NextCheckpointID].transform.position).magnitude)
        .ToList();
    }

    public void StartRace()
    {
        var track = Track.Instance;
        var startSquares = track.ObjectsParent.GetComponentsInChildren<StartSquare>();
        startSquares.OrderBy(m => m.transform.position - Checkpoints[0].transform.position);
        
        var cars = FindObjectsOfType<Car>();
        for (int i = 0; i < cars.Count(); i++)
        {
            Cars.Add(new LapInfo(cars[i]));
            cars[i].transform.position = startSquares[i % (startSquares.Length - 1)].transform.position;
            cars[i].transform.rotation = startSquares[i % (startSquares.Length - 1)].transform.rotation;
        }

        FindObjectOfType<LapTrackerUI>().Init();
        raceStartTime = Time.time;

        StartCoroutine(StartCountDown());
    }

    private IEnumerator StartCountDown()
    {
        GameManager.SetState(State.Game);
        yield return null;
    }

    public void EnterCheckpoint(Car car, int checkpointID)
    {
        var lapInfo = GetCarLapInfo(car.Player.ID);

        if (lapInfo.NextCheckpointID == checkpointID)
        {
            lapInfo.CurrentCheckpointID = checkpointID;

            if (lapInfo.CurrentCheckpointID == 0)
            {
                lapInfo.CurrentLap++;
                lapInfo.LapTimes.Add(Time.time - raceStartTime);
            }
        }
    }

    private LapInfo GetCarLapInfo(Guid ID)
    {
        return Cars.Find(x => x.car.Player.ID == ID);
    }
}

public class LapInfo
{
    public Car car;
    public int CurrentLap;
    public int CurrentCheckpointID;
    public List<float> LapTimes;

    public float RaceTotalTime { get { return LapTimes.Count() > 0 ? LapTimes.Sum() : 0; } }

    public float LastLapTime { get { return LapTimes.Count() > 0 ? LapTimes.Last() : 0; } }

    public float FastestLapTime { get { return LapTimes.Count() > 0 ? LapTimes.Min() : 0 ; } }

    public int NextCheckpointID
    {
        get
        {
            return CurrentCheckpointID < LapTracker.Instance.Checkpoints.Length - 1 ? CurrentCheckpointID + 1 : 0;
        }
    }

    public LapInfo(Car car)
    {
        this.car = car;
        CurrentLap = 1;
        CurrentCheckpointID = 0;

        LapTimes = new List<float>();
    }
}