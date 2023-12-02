using UnityEditor;
using UnityEngine;

namespace LevelData
{
    [CustomEditor(typeof(LevelDetailsData))]
    public class LevelDetailsDataEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var script = (LevelDetailsData)target;

            if (GUILayout.Button("Set Easy Level Random Data", GUILayout.Height(20))) 
                script.SetEasyLevelRandomData();

            if (GUILayout.Button("Set Medium Level Random Data", GUILayout.Height(20)))
                script.SetMediumLevelRandomData();

            if (GUILayout.Button("Set Hard Level Random Data", GUILayout.Height(20))) 
                script.SetHardLevelRandomData();

            if (GUILayout.Button("Set Super Hard Level Random Data", GUILayout.Height(20)))
                script.SetSuperHardRandomData();
        }
    }
}