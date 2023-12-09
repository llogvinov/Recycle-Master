using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Core.SaveService
{
    public class BinarySaveSystem<T> : ISaveSystem<T>
    {
        private readonly string _filePath;

        public BinarySaveSystem()
        {
            _filePath = Application.persistentDataPath + "/PlayerProgress.dat";
        }
        
        public void Save(T data)
        {
            using (FileStream fileStream = File.Create(_filePath))
            {
                new BinaryFormatter().Serialize(fileStream, data);
            }
        }

        public T Load()
        {
            T result;
            using (FileStream fileStream = File.Open(_filePath, FileMode.Open))
            {
                var loaded = new BinaryFormatter().Deserialize(fileStream);
                result = (T) loaded;
            }

            return result;
        }
    }
}