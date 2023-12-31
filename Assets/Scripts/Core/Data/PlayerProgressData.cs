using System;

namespace Core.Data
{
    [Serializable]
    public class PlayerProgressData
    {
        public int CurrentLevel { get; set; } = 1;
        public bool TutorialCompleted { get; set; } = false;
    }
}