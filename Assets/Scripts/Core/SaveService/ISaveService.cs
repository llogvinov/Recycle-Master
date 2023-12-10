namespace Core.SaveService
{
    public interface ISaveService<T> : IService
    {
        T SaveData { get; }
        void Save(T data);
        T Load();
    }
}