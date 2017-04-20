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

	public void Init()
    {
        lapTracker = Track.Instance.LapTracker;
        track = Track.Instance;

        MapName.text = track.Name;
        LapRecord.text = "TR:" + "T0:D0";

        Laps.text = lapTracker.Laps + " laps";

        foreach (var car in lapTracker.Cars)
        {
            var obj = Instantiate(CarLapInfoPrefab) as GameObject;
            CarLapInfoPrefab.GetComponent<CarInfo>().Init(car);

            obj.transform.SetParent(CarInfoParent, true);
            obj.GetComponent<RectTransform>().sizeDelta = new Vector2(CarInfoParent.rect.width / lapTracker.Cars.Count, 0);
        }
	}
}
