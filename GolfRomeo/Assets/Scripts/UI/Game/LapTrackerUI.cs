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

        ClearChildrens();

        lapTracker = Track.Instance.LapTracker;
        track = Track.Instance;

        MapName.text = track.Name;
        LapRecord.text = "TR:" + "todo";

        carInfoUIs = new List<CarInfo>();

        foreach (var car in lapTracker.CarOrder)
        {
            var obj = Instantiate(CarLapInfoPrefab) as GameObject;
            obj.GetComponent<CarInfo>().Init(car);
            carInfoUIs.Add(obj.GetComponent<CarInfo>());

            obj.transform.SetParent(CarInfoParent, true);
            obj.GetComponent<RectTransform>().sizeDelta = new Vector2(CarInfoParent.rect.width / lapTracker.CarOrder.Count, 0);
        }
	}

    public void Hide()
    {
        ClearChildrens();
        gameObject.SetActive(false);
    }

    public void ClearChildrens()
    {
        var oldLapInfos = CarInfoParent.GetComponentsInChildren<CarInfo>();
        for (int i = 0; i < oldLapInfos.Length; i++)
        {
            Destroy(oldLapInfos[i].gameObject);
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
