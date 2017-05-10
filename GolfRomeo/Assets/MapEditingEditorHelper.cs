using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MapEditingEditorHelper : MonoBehaviour
{
#if UNITY_EDITOR
	void Update ()
    {
        if (Selection.activeGameObject != null && Selection.activeGameObject.GetComponentInParent<TrackObject>() != null) 
        {
            Selection.activeGameObject = Selection.activeGameObject.GetComponentInParent<TrackObject>().gameObject;
        }
    }
#endif
}
