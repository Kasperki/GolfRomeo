using UnityEngine;

public class Car : MonoBehaviour
{
    public Player Player;

    public Renderer PrimaryColor;
    public Renderer SecondaryColor;

    public float Fuel;
    public float FuelUsage;
    public float MaxFuel;

    public float Health;
    public float MaxHealth;

    public void Init(Player player)
    {
        Player = player;
        PrimaryColor.material.color = Player.PrimaryColor;
        SecondaryColor.material.color = Player.SecondaryColor;
    }
}