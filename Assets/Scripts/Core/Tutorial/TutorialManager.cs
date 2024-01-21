using System;
using System.Collections;
using System.Collections.Generic;

namespace Core.Tutorial
{
    public class TutorialManager
    {
        public event Action TutorialCompleted;
        
        private Queue<ITutorialPart> _tutorialParts = new();
        
        public static TutorialManager Create() 
        {
            return new TutorialManager();
        }

        public TutorialManager AddPart(ITutorialPart part)
        {
            _tutorialParts.Enqueue(part);
            return this;
        }

        public IEnumerator StartExecution()
        {
            while (_tutorialParts.Count > 0)
            {
                var currentPart = _tutorialParts.Dequeue();
                currentPart.OnExecuted += () =>
                {
                    if (_tutorialParts.Count == 0)
                        TutorialCompleted?.Invoke();
                };
                yield return currentPart.WaitForAction();
            }
        }
    }
}