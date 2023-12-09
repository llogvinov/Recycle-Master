using Core.Data;

namespace Core.SaveService
{
    public interface ISaveSystem<T>
    {
        void Save(T data);
        T Load();
    }
}