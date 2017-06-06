using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LapTracker : Singleton<LapTracker>
{
    public LapTrackerUI LapTrackerUI;

    public Checkpoint[] Checkpoints { get { return GetComponentsInChildren<Checkpoint>(); } }
    public List<CarRaceData> CarOrder;

    private float raceStartTime;
    private float timeUntilRaceIsOver;
    private bool someoneHasFinishedRace;
    private const float Time_Left_After_First_Finish = 20;

    void Start()
    {
        CarOrder = new List<CarRaceData>();
    }

    public void Initialize(Car[] cars)
    {
        GameManager.SetState(State.Pause);
        someoneHasFinishedRace = false;

        StartCoroutine(InvokeNextFrame(cars));
    }

    private IEnumerator InvokeNextFrame(Car[] cars)
    {
        yield return null;
        SetupCarsAndRaceData(cars);
        yield return null;

        foreach (var carOrder in CarOrder)
        {
            carOrder.car.GetComponent<Rigidbody>().isKinematic = false;
        }

        LapTrackerUI.Init();
        StartCoroutine(StartCountDown());
    }

    private void SetupCarsAndRaceData(Car[] cars)
    {
        CarOrder.Clear();

        var startSquares = Track.Instance.TrackObjectsParent.GetComponentsInChildren<StartSquare>();
        startSquares.OrderBy(m => m.transform.position - Checkpoints[0].transform.position);

        for (int i = 0; i < cars.Length; i++)
        {
            CarOrder.Add(new CarRaceData(cars[i]));

            var squareIndex = i % (startSquares.Length);
            cars[i].transform.position = startSquares[squareIndex].transform.position + new Vector3(0, 0.1f, 0);
            cars[i].transform.rotation = startSquares[squareIndex].transform.rotation;
            cars[i].GetComponent<Rigidbody>().isKinematic = true;
        }
    }

    private IEnumerator StartCountDown()
    {
        FindObjectOfType<CountDown>().Awake();
        var startTime = Time.time + 3.5f;

        while (Time.time < startTime)
        {
            FindObjectOfType<CountDown>().UpdateCountdown(startTime - Time.time - 0.5f);
            yield return null;
        }

        StartRace();
        yield return null;
    }

    private void StartRace()
    {
        GameManager.SetState(State.Game);
        raceStartTime = Time.time;
    }

    void Update()
    {
        if (GameManager.CheckState(State.Game))
        {
            //Update car positions
            CarOrder = CarOrder.OrderByDescending(x => x.Finished ? 1 : 0)
            .ThenByDescending(x => x.CurrentLap)
            .ThenByDescending(x => x.CurrentCheckpointID)
            .ThenBy(x => x.RaceTotalTime)
            .ThenBy(x => (x.car.transform.position - Checkpoints[x.NextCheckpointID].transform.position).magnitude)
            .ToList();

            //Check when race ends
            if (CarOrder.Find(x => x.Finished == true) != null && someoneHasFinishedRace == false)
            {
                someoneHasFinishedRace = true;
                timeUntilRaceIsOver = Time.time + Time_Left_After_First_Finish;
            }

            if (someoneHasFinishedRace && Time.time > timeUntilRaceIsOver)
            {
                RaceManager.Instance.EndRace();
            }

            if (CarOrder.FindAll(x => x.Finished == true).Count == CarOrder.Count)
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
        return CarOrder.FindIndex(x => x.car.Player.ID == ID);
    }

    private CarRaceData GetCarLapInfo(Guid ID)
    {
        return CarOrder.Find(x => x.car.Player.ID == ID);
    }
}