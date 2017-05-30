using UnityEditor;
using UnityEditor.UI;

/// <summary>
/// Based on: http://answers.unity3d.com/answers/1227320/view.html
/// </summary>
[CustomEditor(typeof(HoldButton), true)]
public class HoldButtonEditor : ButtonEditor
{
    SerializedProperty _onDownProperty;

    protected override void OnEnable()
    {
        base.OnEnable();
        _onDownProperty = serializedObject.FindProperty("_onDown");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EditorGUILayout.Space();

        serializedObject.Update();
        EditorGUILayout.PropertyField(_onDownProperty);
        serializedObject.ApplyModifiedProperties();
    }
}