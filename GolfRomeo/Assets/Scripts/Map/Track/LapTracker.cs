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
            .ThenBy(x => x.Value.CurrentCheckpointID)
            .ThenBy(x => Checkpoints[x.Value.CurrentCheckpointID].transform.position - Checkpoints[x.Value.NextCheckpointID].transform.position);

        if (Input.GetKeyDown(KeyCode.K))
        {
            string msg = "";
            int index = 1;

            foreach (var car in Cars)
            {
                msg += index++ + ": " + car.Key + " lap:" + car.Value.CurrentLap + " chk:" + car.Value.CurrentCheckpointID + " next:;" + car.Value.NextCheckpointID + "\n";
            }

            Debug.Log(msg);
        }
    }

    public void EnterCheckpoint(string name, int checkpointID)
    {
        Debug.Log("car:" + name + "is on " + checkpointID);

        if (Cars[name].NextCheckpointID == checkpointID)
        {
            Cars[name].CurrentCheckpointID = checkpointID;

            if (Cars[name].CurrentCheckpointID == 0)
            {
                Cars[name].CurrentLap++;

                Debug.Log("car:" + name + "is on lap " + Cars[name].CurrentLap);
            }
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
            return CurrentCheckpointID < LapTracker.Instance.Checkpoints.Length - 1 ? CurrentCheckpointID + 1 : 0;
        }
    }

    public LapInfo()
    {
        CurrentLap = 1;
        CurrentCheckpointID = 0;
    }
}