using UnityEditor;
using UnityEditor.UI;

/// <summary>
/// Based on: http://answers.unity3d.com/answers/1227320/view.html
/// </summary>
[CustomEditor(typeof(HoldButton), true)]
public class HoldButtonEditor : ButtonEditor
{
    private SerializedProperty onDownProperty;

    protected override void OnEnable()
    {
        base.OnEnable();
        onDownProperty = serializedObject.FindProperty("_onDown");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EditorGUILayout.Space();

        serializedObject.Update();
        EditorGUILayout.PropertyField(onDownProperty);
        serializedObject.ApplyModifiedProperties();
    }
}