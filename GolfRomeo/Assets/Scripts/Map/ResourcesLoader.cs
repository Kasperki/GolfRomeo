using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesLoader
{
    public const string ROADS = "Roads";
    public const string TRACKOBJECTS = "Objects";
    public const string CARS = "Cars";

    public static GameObject LoadTrackObject(string name)
    {
        return UnityEngine.GameObject.Instantiate(Resources.Load(TRACKOBJECTS + "/" + name)) as GameObject;
    }

    public static GameObject LoadRoadObject(string name)
    {
        return UnityEngine.GameObject.Instantiate(Resources.Load(ROADS + "/" + name)) as GameObject;
    }

    public static GameObject LoadCar(CarType CarType)
    {
        return UnityEngine.GameObject.Instantiate(Resources.Load(CARS + "/" + CarType.ToString())) as GameObject;
    }
}
