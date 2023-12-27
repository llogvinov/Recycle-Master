using System;
using System.Threading.Tasks;
using LevelData;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Main.Level
{
    public static class CachedLevel
    {
        public static LevelDetailsData CurrentLevelDetailsData;

        private const byte LoadAttempts = 3;
        private const byte LoadTimeout = 3;
        
        public static async Task CacheLevel(int levelNumber)
        {
            var handle = Addressables.LoadAssetAsync<LevelDetailsData>($"Level {levelNumber.ToString()}");

            var attempt = 1;
            do
            {
                await handle.Task;
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    var loadedObject = handle.Result;
                    if (loadedObject is null)
                    {
                        throw new NullReferenceException(
                            $"Object of type {typeof(LevelDetailsData)} is null on attempt to load it from addressables with key \"Level {levelNumber}\"");
                    }
                }

                await Task.Delay(LoadTimeout * 1000);
            } while (++attempt < LoadAttempts);
        }
    }
}