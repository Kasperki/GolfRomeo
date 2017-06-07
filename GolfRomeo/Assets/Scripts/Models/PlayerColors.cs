using UnityEngine;

class PlayerColors
{
    public static Color[] PrimaryColors
    {
        get
        {
            return new Color[RaceManager.MaxPlayers] { Color.red, Color.blue, Color.green, Color.yellow, Color.magenta, new Color32(255, 125, 0, 1), Color.white, Color.gray };
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