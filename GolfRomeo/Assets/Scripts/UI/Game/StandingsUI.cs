using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StandingsUI : MonoBehaviour
{
    public StandingUIPlayerCard[] StandingsObject;
    public RectTransform StandingsParent;

    public void SetStandings(Dictionary<Player, int> standings)
    {
        gameObject.SetActive(true);

        int i = 0;
        foreach (var standing in standings)
        {
            StandingsObject[i].gameObject.SetActive(true);
            StandingsObject[i++].UpdateCardInfo(standing.Key, standing.Value);
        }

        for (int j = i; j < StandingsObject.Length; j++)
        {
            StandingsObject[j].gameObject.SetActive(false);
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
