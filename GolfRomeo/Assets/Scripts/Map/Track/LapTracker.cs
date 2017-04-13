using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class LapTracker : Singleton<LapTracker>
{
    public int Laps;
    public Checkpoint[] Checkpoints { get { return GetComponentsInChildren<Checkpoint>(); } }
    public List<LapInfo> Cars;

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

    public int GetPosition(string name)
    {
        return Cars.FindIndex(x => x.car.Name == name);
    }

    void Start()
    {
        Cars = new List<LapInfo>();
        StartRace();
    }

    void StartRace()
    {
        foreach(var car in FindObjectsOfType<Car>())
        {
            Cars.Add(new LapInfo(car));
        }
    }

    void Update()
    {
        Cars = Cars.OrderByDescending(x => x.CurrentLap)
            .ThenByDescending(x => x.CurrentCheckpointID)
            .ThenBy(x => (x.car.transform.position - Checkpoints[x.NextCheckpointID].transform.position).magnitude)
            .ToList();
    }

    public void EnterCheckpoint(string name, int checkpointID)
    {
        var lapInfo = GetCarLapInfo(name);

        if (lapInfo.NextCheckpointID == checkpointID)
        {
            lapInfo.CurrentCheckpointID = checkpointID;

            if (lapInfo.CurrentCheckpointID == 0)
            {
                lapInfo.CurrentLap++;
            }
        }
    }

    private LapInfo GetCarLapInfo(string name)
    {
        return Cars.Find(x => x.car.Name == name);
    }
}

public class LapInfo
{
    public Car car;
    public int CurrentLap;
    public int CurrentCheckpointID;

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
    }
}