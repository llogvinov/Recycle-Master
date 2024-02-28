using System.IO;
using Core.Data;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class ResetManager : MonoBehaviour
    {
        private static readonly string PlayerProgressFilePath = ConstructFilePath($"{nameof(PlayerProgressData)}");
        private static readonly string PlayerSettingsFilePath = ConstructFilePath($"{nameof(PlayerSettingsData)}");
        
        private static string ConstructFilePath(string fileName) => 
            Path.Combine(Application.persistentDataPath, $"{fileName}.dat");

        [MenuItem("RecycleMaster/Reset Saved Data")]
        public static void ResetSavedData()
        {
            File.Delete(PlayerProgressFilePath);
            File.Delete(PlayerSettingsFilePath);
        }
    }
}