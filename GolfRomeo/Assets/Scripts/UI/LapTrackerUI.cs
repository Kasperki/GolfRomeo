using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LapTrackerUI : MonoBehaviour
{
    private LapTracker lapTracker;
    private Track track;

    public Text MapName;
    public Text LapRecord;
    public Text Laps;

    public RectTransform CarInfoParent;
    public GameObject CarLapInfoPrefab;

    private List<CarInfo> carInfoUIs;

	public void Init()
    {
        gameObject.SetActive(true);

        lapTracker = Track.Instance.LapTracker;
        track = Track.Instance;

        MapName.text = track.Name;
        LapRecord.text = "TR:" + "T0:D0";

        Laps.text = lapTracker.Laps + " laps";

        carInfoUIs = new List<CarInfo>();

        foreach (var car in lapTracker.Cars)
        {
            var obj = Instantiate(CarLapInfoPrefab) as GameObject;
            obj.GetComponent<CarInfo>().Init(car);
            carInfoUIs.Add(obj.GetComponent<CarInfo>());

            obj.transform.SetParent(CarInfoParent, true);
            obj.GetComponent<RectTransform>().sizeDelta = new Vector2(CarInfoParent.rect.width / lapTracker.Cars.Count, 0);
        }
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
