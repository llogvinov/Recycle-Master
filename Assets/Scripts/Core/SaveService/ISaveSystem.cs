using Core.Data;

namespace Core.SaveService
{
    public interface ISaveSystem
    {
        void Save(PlayerProgress data);
        PlayerProgress Load();
    }
}