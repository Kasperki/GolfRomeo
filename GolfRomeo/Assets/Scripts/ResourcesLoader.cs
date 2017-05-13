using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesLoader : MonoBehaviour
{
    public const string ROADS = "Roads";
    public const string TRACKOBJECTS = "Objects";
    public const string CARS = "Cars";

    public static GameObject LoadTrackObject(string name)
    {
        return Instantiate(Resources.Load(TRACKOBJECTS + "/" + name)) as GameObject;
    }

    public static GameObject LoadRoadObject(string name)
    {
        return Instantiate(Resources.Load(ROADS + "/" + name)) as GameObject;
    }

    public static GameObject LoadCar(CarType CarType)
    {
        return Instantiate(Resources.Load(CARS + "/" + CarType.ToString())) as GameObject;
    }
}
