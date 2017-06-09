using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StandingsUI : MonoBehaviour
{
    public RectTransform StandingsParent;
    public StandingUIPlayerCard[] StandingsObject;

    public RectTransform StadningsWinnersParent;
    public StandingUIPlayerCard[] StandingsObjectWinner;

    public void SetStandings(Dictionary<Player, StandingsData> standings)
    {
        gameObject.SetActive(true);
        StandingsParent.gameObject.SetActive(true);
        StadningsWinnersParent.gameObject.SetActive(false);

        int i = 0;
        foreach (var standing in standings)
        {
            StandingsObject[i++].UpdateCardInfo(standing.Key, standing.Value);
        }

        for (int j = standings.Count; j < StandingsObject.Length; j++)
        {
            StandingsObject[j].HideCard();
        }
    }

    public void ShowWinners(Dictionary<Player, StandingsData> standings)
    {
        gameObject.SetActive(true);
        StandingsParent.gameObject.SetActive(false);
        StadningsWinnersParent.gameObject.SetActive(true);

        int i = 0;
        foreach (var standing in standings)
        {
            StandingsObjectWinner[i++].UpdateWinnerCardInfo(standing.Key, standing.Value);
        }

        for (int j = standings.Count; j < StandingsObjectWinner.Length; j++)
        {
            StandingsObjectWinner[j].HideCard();
        }
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
