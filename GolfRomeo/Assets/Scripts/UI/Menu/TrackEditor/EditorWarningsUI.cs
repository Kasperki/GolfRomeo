using UnityEngine;
using UnityEngine.UI;

public class EditorWarningsUI : MonoBehaviour
{
    private Text text;

    void Awake()
    {
        text = GetComponent<Text>();
    }

    void Update ()
    {
        text.text = string.Empty;

        if (Track.Instance.LapTracker.Checkpoints.Length == 0)
        {
            text.text += "<color='orange'>! NO CHECKPOINTS !</color> \n";
        }

        if (Track.Instance.WayPointCircuit.waypointList.items.Length == 0)
        {
            text.text += "<color='orange'>! AI HAS NO WAYPOINTS !</color> \n";
        }

        float startPointCount = 0;
        foreach (var trackObject in Track.Instance.MapObjects)
        {
            if (trackObject.GetComponent<StartSquare>() != null)
            {
                startPointCount++;
            }
        }

        if (startPointCount < RaceManager.MaxPlayers)
        {
            text.text += "<color='orange'>You have too little "+startPointCount+"/"+ RaceManager.MaxPlayers + " start points</color> \n";
        }
        else if(startPointCount > RaceManager.MaxPlayers)
        {
            text.text += "<color='orange'>You have too many " + startPointCount + "/" + RaceManager.MaxPlayers + " start points</color> \n";
        }
    }
}
