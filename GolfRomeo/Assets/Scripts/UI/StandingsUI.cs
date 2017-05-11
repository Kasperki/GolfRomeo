using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StandingsUI : MonoBehaviour
{
    public GameObject[] StandingsObject;
    public RectTransform StandingsParent;

    public void SetStandings(Dictionary<Player, int> standings)
    {
        StandingsParent.gameObject.SetActive(true);

        int i = 0;
        foreach (var standing in standings)
        {
            StandingsObject[i++].GetComponent<Text>().text = standing.Key.Name + " : " + standing.Value;
        }
    }

    public void ShowWinners()
    {
        //TODO
    }
}
