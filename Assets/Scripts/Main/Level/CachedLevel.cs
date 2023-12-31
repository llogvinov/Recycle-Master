using System;
using System.Threading.Tasks;
using LevelData;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Main.Level
{
    public static class CachedLevel
    {
        public static LevelDetailsData CurrentLevelDetailsData;

        private const byte LoadAttempts = 2;
        private const byte LoadTimeout = 1;
        
        public static async Task CacheLevel(int levelNumber)
        {
            var assetKey = "Level " + levelNumber;
            var handle = Addressables.LoadAssetAsync<LevelDetailsData>(assetKey);

            var attempt = 1;
            do
            {
                await handle.Task;
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    CurrentLevelDetailsData = handle.Result;
                }
                if (handle.Status == AsyncOperationStatus.Failed)
                {
                    CurrentLevelDetailsData = null;
                    Debug.LogWarning($"Object of type {typeof(LevelDetailsData)} is null" +
                                     $" on attempt to load it from addressables with key \"{assetKey}\"");
                }

                await Task.Delay(LoadTimeout * 1000);
            } while (++attempt < LoadAttempts);
        }
    }
}