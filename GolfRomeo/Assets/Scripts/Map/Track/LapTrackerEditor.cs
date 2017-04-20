using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LapTrackerEditor : MonoBehaviour
{
    public Checkpoint[] Checkpoints { get { return GetComponentsInChildren<Checkpoint>(); } }
    private bool listenToChildCount;
    private int listenedChildNodes;

    void Update ()
    {
	    if (GameManager.CheckState(State.Edit))
        {
            if (listenToChildCount == true)
            {
                if (listenedChildNodes != Checkpoints.Length)
                {
                    listenToChildCount = false;
                    ReOrderCheckpoints();
                }
            }
        }
    }

    public void ListenToReOrderOnChange()
    {
        listenToChildCount = true;
        listenedChildNodes = Checkpoints.Length;
    }

    private void ReOrderCheckpoints()
    {
        for (int i = 0; i < Checkpoints.Length; i++)
        {
            Checkpoints[i].SetOrder(i);
        }
    }
}
