using Core.Data;
using UnityEngine;

namespace Core.SaveService
{
    public class PlayerPrefsSystem : ISaveSystem
    {
        private const string LEVEL_KEY = "Level";
        
        public void Save(PlayerProgress data)
        {
            PlayerPrefs.SetInt(LEVEL_KEY, data.CurrentLevel);
            PlayerPrefs.Save();
        }

        public PlayerProgress Load()
        {
            var result = new PlayerProgress();
            if (PlayerPrefs.HasKey(LEVEL_KEY))
                result.CurrentLevel = PlayerPrefs.GetInt(LEVEL_KEY);

            return result;
        }
    }
}