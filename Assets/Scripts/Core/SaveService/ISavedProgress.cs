using Core.Data;

namespace Core.SaveService
{
    public interface ISavedProgressReader
    {
        void LoadProgress(PlayerProgressService playerProgressService);
    }

    public interface ISavedProgress : ISavedProgressReader
    {
        void UpdateProgress(PlayerProgressService playerProgressService);
    }
}