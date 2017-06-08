using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LapTrackerUI : MonoBehaviour
{
    private LapTracker lapTracker;
    private Track track;

    public Text MapName;
    public Text LapRecord;

    public RectTransform CarInfoParent;
    public GameObject CarLapInfoPrefab;

    private List<CarInfo> carInfoUIs;

	public void Init()
    {
        gameObject.SetActive(true);

        CarInfoParent.DestroyChildrens();

        lapTracker = Track.Instance.LapTracker;
        track = Track.Instance;

        MapName.text = track.Metadata.Name;
        LapRecord.text = "TR: " + TimeSpan.FromSeconds(track.Metadata.TrackRecord).GetTimeInMinutesAndSeconds();

        carInfoUIs = new List<CarInfo>();

        foreach (var car in lapTracker.CarOrder)
        {
            var obj = Instantiate(CarLapInfoPrefab) as GameObject;
            obj.GetComponent<CarInfo>().Init(car);
            carInfoUIs.Add(obj.GetComponent<CarInfo>());

            obj.transform.SetParent(CarInfoParent, true);
            obj.GetComponent<RectTransform>().sizeDelta = new Vector2(CarInfoParent.rect.width / lapTracker.CarOrder.Count, 0);
        }

        //CarInfoParent.GetComponent<HorizontalLayoutGroup>().childAlignment = TextAnchor.MiddleLeft; TODO
    }

    public void Hide()
    {
        CarInfoParent.DestroyChildrens();
        gameObject.SetActive(false);
    }

    public void Update()
    {
        if (carInfoUIs != null)
        {
            foreach (var carInfo in carInfoUIs)
            {
                carInfo.transform.SetSiblingIndex(lapTracker.GetCarPosition(carInfo.LapInfo.car.Player.ID));
            }
        }
    }
}
