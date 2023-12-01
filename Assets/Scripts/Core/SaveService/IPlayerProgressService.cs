using Core.Data;

namespace Core.SaveService
{
    public interface IPlayerProgressService : IService
    {
        PlayerProgress PlayerProgress { get; set; }
    }
}