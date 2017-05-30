using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LapInfo
{
    public Car car;
    public bool Finished;
    public int CurrentLap;
    public int CurrentCheckpointID;
    public List<float> LapTimes;

    public float RaceTotalTime { get { return LapTimes.Count() > 0 ? LapTimes.Sum() : 0; } }

    public float LastLapTime { get { return LapTimes.Count() > 0 ? LapTimes.Last() : 0; } }

    public float FastestLapTime { get { return LapTimes.Count() > 0 ? LapTimes.Min() : 0; } }

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