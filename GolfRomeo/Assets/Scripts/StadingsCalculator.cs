using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StadingsCalculator
{
    public RacePointTemplate RacePointTemplate;
    public Dictionary<Player, int> PlayerStandings;

	public StadingsCalculator(List<Player> players)
    {
        RacePointTemplate = RacePointTemplate.DefaultTemplate;

        PlayerStandings = new Dictionary<Player, int>();
        foreach (var player in players)
        {
            PlayerStandings.Add(player, 0);
        }
    }

    public void UpdateStandings(List<LapInfo> lapinfo)
    {
        for (int i = 0; i < lapinfo.Count; i++)
        {
            PlayerStandings[lapinfo[i].car.Player] += RacePointTemplate.PointOrder[i];
        }

        var fastestPlayer = lapinfo.OrderBy(x => x.FastestLapTime).First().car.Player;
        PlayerStandings[fastestPlayer] += RacePointTemplate.FastestLap;

        SortDictionaryByValue();
    }

    private void SortDictionaryByValue()
    {
        var sortedDict =  from entry in PlayerStandings orderby entry.Value ascending select entry;
        PlayerStandings = sortedDict.ToDictionary(pair => pair.Key, pair => pair.Value);
    }
}
