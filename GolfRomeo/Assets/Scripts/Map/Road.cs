using BLINDED_AM_ME;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[RequireComponent(typeof(Path_Comp))]
[RequireComponent(typeof(Trail_Mesh))]
public class Road : MonoBehaviour
{
    public string ID;
    public bool IsCircuit
    {
        get { return GetComponent<Path_Comp>().isCircuit; }
        set { GetComponent<Path_Comp>().isCircuit = value; }
    }

    private bool listenToChildCount;
    private int listenedChildNodes;

    public RoadNode[] RoadNodes { get { return GetComponentsInChildren<RoadNode>(); } }

    public void GenerateRoadMesh()
    {
        GetComponent<Path_Comp>().Update_Path();
        GetComponent<Trail_Mesh>().ShapeIt();
    }

    public void GenerateRoadMeshOnNextChange()
    {
        listenToChildCount = true;
        listenedChildNodes = RoadNodes.Length;
    }

    private void Update()
    {
        if (GameManager.CheckState(State.Edit))
        {
            if (listenToChildCount == true)
            {
                if (listenedChildNodes != RoadNodes.Length)
                {
                    listenToChildCount = false;
                    GenerateRoadMesh();
                }
            }
        }
    }
}