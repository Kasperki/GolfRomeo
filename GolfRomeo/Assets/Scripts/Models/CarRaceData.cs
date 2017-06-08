using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarRaceData
{
    public Car car;
    public bool Finished;
    public int CurrentLap;
    public int CurrentCheckpointID;
    public List<float> LapTimes;

    private float raceStartTime;

    public float RaceTotalTime { get { return LapTimes.Count() > 0 ? LapTimes.Sum() : 0; } }

    public float LastLapTime { get { return LapTimes.Count() > 0 ? LapTimes.Last() : 0; } }

    public float FastestLapTime { get { return LapTimes.Count() > 0 ? LapTimes.Min() : 0; } }

    public float CurrentLapTime { get { return Time.time - raceStartTime - RaceTotalTime; } }

    public int NextCheckpointID
    {
        get
        {
            return CurrentCheckpointID < LapTracker.Instance.Checkpoints.Length - 1 ? CurrentCheckpointID + 1 : 0;
        }
    }

    public CarRaceData(Car car)
    {
        this.car = car;
        CurrentLap = 1;
        CurrentCheckpointID = 0;

        LapTimes = new List<float>();
    }

    public void SetRaceStartTime(float raceStartTime)
    {
        this.raceStartTime = raceStartTime;
    }
}