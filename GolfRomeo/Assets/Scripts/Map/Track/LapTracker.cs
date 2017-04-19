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

    private float raceStartTime;
    private bool listenToChildCount;
    private int listenedChildNodes;

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
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            StartRace();
        }

        if (GameManager.CheckState(State.Game))
        {
            Cars = Cars.OrderByDescending(x => x.CurrentLap)
            .ThenByDescending(x => x.CurrentCheckpointID)
            .ThenBy(x => (x.car.transform.position - Checkpoints[x.NextCheckpointID].transform.position).magnitude)
            .ToList();
        }
        else if (GameManager.CheckState(State.Edit))
        {
            if (listenToChildCount == true)
            {
                if (listenedChildNodes != Checkpoints.Length)
                {
                    listenToChildCount = false;
                    ReOrderCheckpoints();
                }
            }
        }
    }

    void StartRace()
    {
        GameManager.SetState(State.Game);

        var map = GetComponentInParent<Map>();
        var startSquares = map.ObjectsParent.GetComponentsInChildren<StartSquare>();
        startSquares.OrderBy(m => m.transform.position - Checkpoints[0].transform.position);
        
        var cars = FindObjectsOfType<Car>();

        for (int i = 0; i < cars.Count(); i++)
        {
            Cars.Add(new LapInfo(cars[i]));
            cars[i].transform.position = startSquares[i % (startSquares.Length - 1)].transform.position;
            cars[i].transform.rotation = startSquares[i % (startSquares.Length - 1)].transform.rotation;
        }

        raceStartTime = Time.time;
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
                lapInfo.LapTimes.Add(Time.time - raceStartTime);
            }
        }
    }

    private LapInfo GetCarLapInfo(string name)
    {
        return Cars.Find(x => x.car.Name == name);
    }

    public void ListenToReOrderOnChange()
    {
        listenToChildCount = true;
        listenedChildNodes = Checkpoints.Length;
    }

    private void ReOrderCheckpoints()
    {
        for (int i = 0; i < Checkpoints.Length; i++)
        {
            Checkpoints[i].SetOrder(i);
        }
    }
}

public class LapInfo
{
    public Car car;
    public int CurrentLap;
    public int CurrentCheckpointID;
    public List<float> LapTimes;

    public float RaceTotalTime { get { return LapTimes.Sum(); } }

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