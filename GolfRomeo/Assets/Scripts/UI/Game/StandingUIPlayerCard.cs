using UnityEngine;
using UnityEngine.UI;

public class StandingUIPlayerCard : MonoBehaviour
{
    public Text PlayerName;
    public Text Score;
    public Text ScoreAdded;

    public Image Background;
    public Image PrimaryColor;

	public void UpdateCardInfo (Player player, StandingsData standingsData)
    {
        PlayerName.text = player.Name;
        Score.text = standingsData.Points.ToString("0");

        if (standingsData.TrackRecord)
        {
            ScoreAdded.text = "TR";
        }
        else
        {
            ScoreAdded.text = "";
        }

        Background.color = new Color(player.PrimaryColor.r, player.PrimaryColor.g, player.PrimaryColor.b, 0.2f);
        PrimaryColor.color = player.PrimaryColor;
    }

    public void UpdateWinnerCardInfo(Player player, StandingsData standingsData)
    {
        PlayerName.text = player.Name;
        Score.text = standingsData.Points.ToString("0");
        Background.color = new Color(player.PrimaryColor.r, player.PrimaryColor.g, player.PrimaryColor.b, 0.2f);
        PrimaryColor.color = player.PrimaryColor;
    }

    public void HideCard()
    {
        PlayerName.text = "";
        Score.text = "";

        if (ScoreAdded != null)
        {
            ScoreAdded.text = "";
        }

        Background.color = new Color(0, 0, 0, 0);
        PrimaryColor.color = new Color(0, 0, 0, 0);
    }
}
