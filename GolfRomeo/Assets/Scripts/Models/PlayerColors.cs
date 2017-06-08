using UnityEngine;

class PlayerColors
{
    public static Color[] PrimaryColors
    {
        get
        {
            return new Color[RaceManager.MaxPlayers] { new Color32(255, 125, 125, 255), new Color32(0, 125, 255, 255), new Color32(125, 255, 125, 255), Color.yellow, Color.magenta, new Color32(255, 125, 0, 255), new Color32(175, 200, 175, 255), Color.black };
        }
    }

    public static Color[] SecondaryColors
    {
        get
        {
            return new Color[RaceManager.MaxPlayers] { Color.white, Color.white, Color.white, Color.white, Color.white, Color.white, Color.white, Color.white };
        }
    }
}