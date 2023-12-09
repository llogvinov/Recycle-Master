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
            {
                script.SetEasyLevelRandomData();
                SaveData(script);
            }

            if (GUILayout.Button("Set Medium Level Random Data", GUILayout.Height(20)))
            {
                script.SetMediumLevelRandomData();
                SaveData(script);
            }

            if (GUILayout.Button("Set Hard Level Random Data", GUILayout.Height(20)))
            {
                script.SetHardLevelRandomData();
                SaveData(script);
            }

            if (GUILayout.Button("Set Super Hard Level Random Data", GUILayout.Height(20)))
            {
                script.SetSuperHardRandomData();
                SaveData(script);
            }
        }

        private void SaveData(Object script)
        {
            EditorUtility.SetDirty(script);
            AssetDatabase.SaveAssetIfDirty(script);
        }
    }
}