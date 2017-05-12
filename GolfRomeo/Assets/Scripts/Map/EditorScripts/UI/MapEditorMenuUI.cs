using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapEditorMenuUI : MonoBehaviour
{
    public EditorUI EditorUI;

    public void DiscardAndExit()
    {
        gameObject.SetActive(false);
        EditorUI.Init();
    }

    public void SaveAndExit()
    {
        Track.Instance.SaveTrack();
        gameObject.SetActive(false);
        EditorUI.Init();
    }
}
