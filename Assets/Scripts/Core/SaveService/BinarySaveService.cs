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
            _filePath = Application.persistentDataPath + "/PlayerProgress.dat";
        }

        public void Save(T data = default)
        {
            using (FileStream fileStream = File.Create(_filePath))
            {
                new BinaryFormatter().Serialize(fileStream, data ?? SaveData);
            }
        }

        public T Load()
        {
            using (FileStream fileStream = File.Open(_filePath, FileMode.Open))
            {
                var loaded = new BinaryFormatter().Deserialize(fileStream);
                SaveData = (T) loaded;
            }
            
            return SaveData;
        }
    }
}