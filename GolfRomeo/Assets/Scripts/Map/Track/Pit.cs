using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Pit : MonoBehaviour
{
    public const float Health = 100;
    public const float Fuel = 300;
    public const float Tires = 40;
    private const float TimeAfterTiresChanged = 1;
    private float timeForTiresChanging;

    private Player playerOnPit;

    public void OnTriggerEnter(Collider other)
    {
        var car = other.GetComponent<Car>();
        if (car != null)
        {
            playerOnPit = car.Player;
            timeForTiresChanging = Time.time + TimeAfterTiresChanged;
        }
    }

    public void OnTriggerStay(Collider other)
    {
        var car = other.GetComponent<Car>();
        if (car != null && playerOnPit == car.Player)
        {
            car.Fuel += Health * Time.deltaTime;
            car.Fuel += Fuel * Time.deltaTime;

            if (Time.time > timeForTiresChanging)
            {
                car.Fuel += Tires;
                timeForTiresChanging += TimeAfterTiresChanged;

            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        var car = other.GetComponent<Car>();
        if (car != null && playerOnPit == car.Player)
        {
            playerOnPit = null;
        }
    }
}
