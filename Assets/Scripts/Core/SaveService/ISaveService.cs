namespace Core.SaveService
{
    public interface ISaveService<T> : IService
    {
        T SaveData { get; }
        void Save(T data = default);
        T Load();
        void Clear();
    }
}