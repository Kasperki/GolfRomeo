using UnityEngine;

[RequireComponent(typeof(CarController))]
public class Car : MonoBehaviour
{
    public Player Player;

    public CarController CarController;
    public Renderer PrimaryColor;
    public Renderer SecondaryColor;

    private float fuel;
    private float health;
    private float tires;

    public float Fuel { get { return fuel; } }
    public float MaxFuel;

    public float Health { get { return health; } }
    public float MaxHealth;

    public float Tires { get { return tires; } }
    public float MaxTires;

    public void Awake()
    {
        CarController = GetComponent<CarController>();
    }

    public void Init(Player player)
    {
        Player = player;
        PrimaryColor.material.color = Player.PrimaryColor;
        SecondaryColor.material.color = Player.SecondaryColor;

        AddHealth(MaxHealth);
        AddFuel(MaxFuel);
        AddTires(MaxTires);

        if (player.PlayerType == PlayerType.AI)
        {
            gameObject.AddComponent<AICarControl>();
        }
        else
        {
            gameObject.AddComponent<UserCarController>();
        }
    }

    public void AddTires(float value)
    {
        if (value < 0)
        {
            value *= RaceManager.Instance.RaceOptions.TiresConsuptionRate;
        }

        tires += value;
        tires = Mathf.Clamp(tires, 0, MaxTires);
    }

    public void AddFuel(float value)
    {
        if (value < 0)
        {
            value *= RaceManager.Instance.RaceOptions.FuelConsuptionRate;
        }

        fuel += value;
        fuel = Mathf.Clamp(fuel, 0, MaxFuel);
    }

    public void AddHealth(float value)
    {
        if (value < 0)
        {
            value *= RaceManager.Instance.RaceOptions.DamageFromEnvironmentRate;
        }

        health += value;
        health = Mathf.Clamp(health, 0, MaxHealth);
    }
}