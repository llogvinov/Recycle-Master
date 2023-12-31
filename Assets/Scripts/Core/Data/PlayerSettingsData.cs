using System;

namespace Core.Data
{
    [Serializable]
    public class PlayerSettingsData
    {
        public bool PlayMusic { get; set; } = true;
        public bool PlaySounds { get; set; } = true;
    }
}