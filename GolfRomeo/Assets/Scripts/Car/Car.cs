using UnityEngine;

[RequireComponent(typeof(CarController))]
public class Car : MonoBehaviour
{
    public Player Player;

    public CarController CarController;
    public Renderer PrimaryColor;
    public Renderer SecondaryColor;

    public float Fuel;
    public float FuelUsage;
    public float MaxFuel;

    public float Health;
    public float MaxHealth;

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

        if (player.PlayerType == PlayerType.AI)
        {
            var aiCarController = gameObject.AddComponent<AICarControl>();
            aiCarController.SetDefaultOptions();

            var targetProvider = gameObject.AddComponent<WaypointTargetProvider>();
            aiCarController.targetProvider = targetProvider;
        }
        else
        {
            gameObject.AddComponent<UserCarController>();
            //TODO INIT CONTROL MECHANISM
        }
    }
}