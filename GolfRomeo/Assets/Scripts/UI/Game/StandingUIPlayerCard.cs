using UnityEngine;
using UnityEngine.UI;

public class StandingUIPlayerCard : MonoBehaviour
{
    public Text PlayerName;
    public Text Score;
    public Text ScoreAdded;

    public Image PrimaryColor;
    public Image SecondaryColor;

	public void UpdateCardInfo (Player player, int score)
    {
        PlayerName.text = player.Name;
        Score.text = score.ToString("0");

        PrimaryColor.color = player.PrimaryColor;
        SecondaryColor.color = player.SecondaryColor;
    }
}
