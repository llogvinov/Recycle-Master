using System;
using System.Collections;

namespace Core.Tutorial
{
    public interface ITutorialPart
    {
        event Action OnExecuted;
        IEnumerator WaitForAction();
    }
}