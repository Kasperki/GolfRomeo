using System;
using UnityEngine;

[Serializable]
public class Player
{
    public Guid ID;
    public string Name;
    public Color PrimaryColor;
    public Color SecondaryColor;

    public PlayerType PlayerType;
    public CarType CarType;

    public ControllerScheme ControllerScheme;

    public Player()
    {
        ID = Guid.NewGuid();
        PlayerType = PlayerType.Player;
    }
}
