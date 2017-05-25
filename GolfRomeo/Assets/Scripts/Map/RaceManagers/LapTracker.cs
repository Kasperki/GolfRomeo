using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LapTracker : Singleton<LapTracker>
{
    public LapTrackerUI LapTrackerUI;

    public Checkpoint[] Checkpoints { get { return GetComponentsInChildren<Checkpoint>(); } }
    public List<LapInfo> Cars;

    private float raceStartTime;
    private float timeUntilRaceIsOver;
    private bool someoneHasFinishedRace;
    private const float TIME_LEFT_AFTER_FIRST_FINISH = 20;

    void Start()
    {
        Cars = new List<LapInfo>();
    }

    public void Initialize(Car[] cars)
    {
        StartCoroutine(InvokeNextFrame(cars));
    }

    private IEnumerator InvokeNextFrame(Car[] cars)
    {
        yield return null;
        InitializeLapTracker(cars);
        yield return null;

        foreach (var car in Cars)
        {
            car.car.GetComponent<Rigidbody>().isKinematic = false;
        }
    }

    private void InitializeLapTracker(Car[] cars)
    {
        Cars.Clear();
        someoneHasFinishedRace = false;

        var track = Track.Instance;
        var startSquares = track.TrackObjectsParent.GetComponentsInChildren<StartSquare>();
        startSquares.OrderBy(m => m.transform.position - Checkpoints[0].transform.position);

        for (int i = 0; i < cars.Count(); i++)
        {
            Cars.Add(new LapInfo(cars[i]));
            cars[i].transform.position = startSquares[i % (startSquares.Length)].transform.position + new Vector3(0, -0.1f, 0);
            cars[i].transform.rotation = startSquares[i % (startSquares.Length)].transform.rotation;
            cars[i].GetComponent<Rigidbody>().isKinematic = true;
        }

        LapTrackerUI.Init();
    }

    public void StartTimer()
    {
        raceStartTime = Time.time;
    }

    void Update()
    {
        if (GameManager.CheckState(State.Game))
        {
            //Update car positions
            Cars = Cars.OrderByDescending(x => x.Finished ? 1 : 0)
            .ThenByDescending(x => x.CurrentLap)
            .ThenByDescending(x => x.CurrentCheckpointID)
            .ThenBy(x => x.RaceTotalTime)
            .ThenBy(x => (x.car.transform.position - Checkpoints[x.NextCheckpointID].transform.position).magnitude)
            .ToList();

            //Check when race ends
            if (Cars.Find(x => x.Finished == true) != null && someoneHasFinishedRace == false)
            {
                someoneHasFinishedRace = true;
                timeUntilRaceIsOver = Time.time + TIME_LEFT_AFTER_FIRST_FINISH;
            }

            if (someoneHasFinishedRace && Time.time > timeUntilRaceIsOver)
            {
                RaceManager.Instance.EndRace();
            }
            else if (Cars.FindAll(x => x.Finished == true).Count == Cars.Count)
            {
                RaceManager.Instance.EndRace();
            }
        }
    }

    public void EnterCheckpoint(Car car, int checkpointID)
    {
        var lapInfo = GetCarLapInfo(car.Player.ID);

        if (lapInfo.NextCheckpointID == checkpointID)
        {
            lapInfo.CurrentCheckpointID = checkpointID;

            if (lapInfo.CurrentCheckpointID == 0 && lapInfo.Finished == false)
            {
                lapInfo.LapTimes.Add(Time.time - raceStartTime);

                if (lapInfo.CurrentLap < RaceManager.Instance.Laps)
                {
                    lapInfo.CurrentLap++;
                }
                else
                {
                    lapInfo.Finished = true;
                }
            }
        }
    }

    public int GetCarPosition(Guid ID)
    {
        return Cars.FindIndex(x => x.car.Player.ID == ID);
    }

    private LapInfo GetCarLapInfo(Guid ID)
    {
        return Cars.Find(x => x.car.Player.ID == ID);
    }
}

public class LapInfo
{
    public Car car;
    public bool Finished;
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