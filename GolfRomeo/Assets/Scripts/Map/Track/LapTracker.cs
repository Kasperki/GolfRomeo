using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class LapTracker : Singleton<LapTracker>
{
    public int Laps;
    public Checkpoint[] Checkpoints { get { return GetComponentsInChildren<Checkpoint>(); } }
    public SortedList<string, LapInfo> Cars;

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
        return Cars.IndexOfKey(name);
    }

    void Start()
    {
        Cars = new SortedList<string, LapInfo>();
        StartRace();
    }

    void StartRace()
    {
        foreach(var car in FindObjectsOfType<Car>())
        {
            Cars.Add(car.Name, new LapInfo());
        }
    }

    void Update()
    {
        Cars.OrderBy(x => x.Value.CurrentLap)
            .OrderBy(x => x.Value.CurrentCheckpointID)
            .OrderBy(x => Checkpoints[x.Value.CurrentCheckpointID].transform.position - Checkpoints[x.Value.NextCheckpointID].transform.position);

        if (Input.GetKeyDown(KeyCode.K))
        {
            string msg = "";

            foreach (var car in Cars)
            {
                msg += "1: " + car.Key + " lap:" + car.Value.CurrentLap + " chk:" + car.Value.CurrentCheckpointID;
            }
        }
    }

    public void EnterCheckpoint(string name, int checkpointID)
    {
        if (Cars[name].NextCheckpointID == checkpointID)
        {
            if (Cars[name].NextCheckpointID == 0)
            {
                Cars[name].CurrentLap++;
            }

            Cars[name].CurrentCheckpointID = checkpointID;
        }
    }
}

public class LapInfo
{
    public int CurrentLap;
    public int CurrentCheckpointID;

    public int NextCheckpointID
    {
        get
        {
            return CurrentCheckpointID < LapTracker.Instance.Checkpoints.Length ? CurrentCheckpointID + 1 : 0;
        }
    }
}