using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Core.SaveService
{
    public class BinarySaveService<T> : ISaveService<T> where T : new()
    {
        private readonly string _filePath;

        public T SaveData { get; private set; }

        public BinarySaveService()
        {
            SaveData = new T();
            _filePath = ConstructFilePath();
        }
        
        private string ConstructFilePath() => 
            Path.Combine(Application.persistentDataPath, $"{typeof(T).Name}.dat");

        public void Save(T data = default)
        {
            try
            {
                using (FileStream fileStream = File.Create(_filePath))
                {
                    new BinaryFormatter().Serialize(fileStream, data ?? SaveData);
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error saving data: {ex.Message}");
            }
        }

        public T Load()
        {
            try
            {
                if (!File.Exists(_filePath))
                {
                    Save();
                    return SaveData;
                }

                using (FileStream fileStream = File.Open(_filePath, FileMode.Open))
                {
                    var loaded = new BinaryFormatter().Deserialize(fileStream);
                    SaveData = (T)loaded;
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error loading data: {ex.Message}");
            }

            return SaveData;
        }
    }
}