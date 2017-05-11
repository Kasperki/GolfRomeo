using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StadingsCalculator
{
    public RacePointTemplate RacePointTemplate;
    public Dictionary<Player, int> PlayerStandings;
    private StandingsUI standingsUI;

	public StadingsCalculator(List<Player> players, StandingsUI standingsUI)
    {
        RacePointTemplate = RacePointTemplate.DefaultTemplate;

        PlayerStandings = new Dictionary<Player, int>();
        foreach (var player in players)
        {
            PlayerStandings.Add(player, 0);
        }

        this.standingsUI = standingsUI;
    }

    public void UpdateStandings(List<LapInfo> lapinfo)
    {
        //Order points
        for (int i = 0; i < lapinfo.Count; i++)
        {
            PlayerStandings[lapinfo[i].car.Player] += RacePointTemplate.PointOrder[i];
        }

        //Fastest lap points
        var fastestPlayer = lapinfo.OrderBy(x => x.FastestLapTime).First().car.Player;
        PlayerStandings[fastestPlayer] += RacePointTemplate.FastestLap;

        //Player did not finish points
        foreach (var linfo in lapinfo.FindAll(x => x.Finished == false).ToList())
        {
            PlayerStandings[linfo.car.Player] -= RacePointTemplate.DidNotFinish;
        }

        SortDictionaryByValue();
    }

    private void SortDictionaryByValue()
    {
        var sortedDict =  from entry in PlayerStandings orderby entry.Value ascending select entry;
        PlayerStandings = sortedDict.ToDictionary(pair => pair.Key, pair => pair.Value);
    }

    public void ShowStandings()
    {
        standingsUI.SetStandings(PlayerStandings);
    }

    public void ShowWinners()
    {
        standingsUI.ShowWinners();
    }
}
