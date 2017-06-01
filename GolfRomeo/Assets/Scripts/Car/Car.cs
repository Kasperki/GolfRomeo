using UnityEngine;

[RequireComponent(typeof(CarController))]
public class Car : MonoBehaviour
{
    public Player Player;

    public CarController CarController;
    public Renderer PrimaryColor;
    public Renderer SecondaryColor;

    public float Fuel;
    public float MaxFuel;

    public float Health;
    public float MaxHealth;

    public float TiresHealth;
    public float TiresMaxHealth;

    public void Awake()
    {
        CarController = GetComponent<CarController>();
    }

    public void Init(Player player)
    {
        Player = player;
        PrimaryColor.material.color = Player.PrimaryColor;
        SecondaryColor.material.color = Player.SecondaryColor;

        Health = MaxHealth;
        Fuel = MaxFuel;
        TiresHealth = TiresMaxHealth;

        if (player.PlayerType == PlayerType.AI)
        {
            var aiCarController = gameObject.AddComponent<AICarControl>();
        }
        else
        {
            gameObject.AddComponent<UserCarController>();
        }
    }
}