using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Pit : TrackObject
{
    public bool CarOnPit
    {
        get { return playerOnPit != null; }
    }

    private const float Health = 2.0f;
    private const float Fuel = 4.5f;
    private const float Tires = 30;
    private const float TimeAfterTiresChanged = 1;
    private float timeForTiresChanging;

    private Player playerOnPit;

    public void OnTriggerEnter(Collider other)
    {
        var car = other.GetComponentInParent<Car>();
        if (car != null)
        {
            playerOnPit = car.Player;
            timeForTiresChanging = Time.time + TimeAfterTiresChanged;
        }
    }

    public void OnTriggerStay(Collider other)
    {
        var car = other.GetComponentInParent<Car>();
        if (car != null && playerOnPit == car.Player)
        {
            car.AddHealth(Health * Time.deltaTime);
            car.AddFuel(Fuel * Time.deltaTime);

            if (Time.time > timeForTiresChanging)
            {
                car.AddTires(Tires);
                timeForTiresChanging += TimeAfterTiresChanged;

            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        var car = other.GetComponentInParent<Car>();
        if (car != null && playerOnPit == car.Player)
        {
            playerOnPit = null;
        }
    }
}
