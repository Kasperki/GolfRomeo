using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesLoader
{
    public const string Roads = "Roads";
    public const string Track_Objects = "Objects";
    public const string Cars = "Cars";

    public static GameObject LoadTrackObject(string name)
    {
        return UnityEngine.GameObject.Instantiate(Resources.Load(Track_Objects + "/" + name)) as GameObject;
    }

    public static GameObject LoadRoadObject(string name)
    {
        return UnityEngine.GameObject.Instantiate(Resources.Load(Roads + "/" + name)) as GameObject;
    }

    public static GameObject LoadCar(CarType CarType)
    {
        return UnityEngine.GameObject.Instantiate(Resources.Load(Cars + "/" + CarType.ToString())) as GameObject;
    }
}
