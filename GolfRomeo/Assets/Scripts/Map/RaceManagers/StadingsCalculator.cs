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

    public void UpdateStandings(List<CarRaceData> lapinfo)
    {
        //Order points
        for (int i = 0; i < lapinfo.Count; i++)
        {
            PlayerStandings[lapinfo[i].car.Player] += RacePointTemplate.PointOrder[i];
        }

        //Fastest lap points
        var lapInfoWithTime = lapinfo.FindAll(x => x.LastLapTime > 0);
        var fastestCarRaceData = lapInfoWithTime.OrderBy(x => x.FastestLapTime).First();

        if (fastestCarRaceData != null)
        {
            var fastestPlayer = fastestCarRaceData.car.Player;
            PlayerStandings[fastestPlayer] += RacePointTemplate.FastestLap;

            //Update TrackRecord
            if (Track.Instance.Metadata.TrackRecord == 0 || Track.Instance.Metadata.TrackRecord > fastestCarRaceData.FastestLapTime)
            {
                var trackXMLEditor = new TrackXMLDataEditor(Track.Instance.Metadata.Name);
                trackXMLEditor.ChangeTrackRecord(fastestCarRaceData.FastestLapTime);
            }
        }

        //Player did not finish points
        foreach (var linfo in lapinfo.FindAll(x => x.Finished == false).ToList())
        {
            PlayerStandings[linfo.car.Player] -= RacePointTemplate.DidNotFinish;
        }

        SortDictionaryByValue();
    }

    private void SortDictionaryByValue()
    {
        var sortedDict =  from entry in PlayerStandings orderby entry.Value descending select entry;
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

    public void HideStandings()
    {
        standingsUI.Hide();
    }
}
