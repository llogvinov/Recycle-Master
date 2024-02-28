using System;
using System.Collections;
using Main.Level;
using ObjectsData;

namespace Core.Tutorial
{
    public class LevelPart : ITutorialPart
    {
        public event Action OnExecuted;
        
        private readonly LevelManager _levelManager;
        private readonly TrashCanData _trashCanData;

        public LevelPart(LevelManager levelManager, TrashCanData trashCanData)
        {
            _levelManager = levelManager;
            _trashCanData = trashCanData;
        }
        
        public IEnumerator WaitForAction()
        {
            var levelCompleted = false;
            
            _levelManager.BuildTutorialLevel(_trashCanData);
            _levelManager.LevelComplete += () => levelCompleted = true;

            while (!levelCompleted)
            {
                yield return null;
            }
            
            OnExecuted?.Invoke();
        }
    }
}