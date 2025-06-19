using UnityEditor;
using UnityEngine;

public class TimeScaleControllerEditor : EditorWindow
{
    private float timeScale = 1f;

    [MenuItem("Tools/Time Scale Controller")]
    public static void ShowWindow()
    {
        GetWindow<TimeScaleControllerEditor>("Time Scale Controller");
    }

    private void OnGUI()
    {
        GUILayout.Label("Game Time Scale Controller", EditorStyles.boldLabel);

        timeScale = EditorGUILayout.Slider("Time Scale", timeScale, 0f, 5f);

        if (GUILayout.Button("Apply Time Scale"))
        {
            Time.timeScale = timeScale;
        }

        if (GUILayout.Button("Reset to Normal"))
        {
            timeScale = 1f;
            Time.timeScale = 1f;
        }

        GUILayout.Space(10);
        EditorGUILayout.LabelField("Current Time.timeScale: " + Time.timeScale.ToString("F2"));
    }
}
