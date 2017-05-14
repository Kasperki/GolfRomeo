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
        gameObject.SetActive(true);

        int i = 0;
        foreach (var standing in standings)
        {
            StandingsObject[i++].GetComponent<Text>().text = standing.Key.Name + " : " + standing.Value;
        }

        for (int j = i; j < StandingsObject.Length; j++)
        {
            StandingsObject[j].GetComponent<Text>().text = "";
        }
    }

    public void ShowWinners()
    {
        //TODO
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
