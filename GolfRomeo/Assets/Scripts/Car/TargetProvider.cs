using System;
using UnityEngine;

public abstract class TargetProvider : MonoBehaviour
{
    public abstract Transform GetTargetTransform();
}