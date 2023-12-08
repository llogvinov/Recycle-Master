using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Core.Data;
using UnityEngine;

namespace Core.SaveService
{
    public class BinarySaveSystem : ISaveSystem
    {
        private readonly string _filePath;

        public BinarySaveSystem()
        {
            _filePath = Application.persistentDataPath + "/PlayerProgress.dat";
        }
        
        public void Save(PlayerProgress data)
        {
            using (FileStream fileStream = File.Create(_filePath))
            {
                new BinaryFormatter().Serialize(fileStream, data);
            }
        }

        public PlayerProgress Load()
        {
            PlayerProgress result;
            using (FileStream fileStream = File.Open(_filePath, FileMode.Open))
            {
                var loaded = new BinaryFormatter().Deserialize(fileStream);
                result = (PlayerProgress) loaded;
            }

            return result;
        }
    }
}