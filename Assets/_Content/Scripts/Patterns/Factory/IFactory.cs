namespace _Content.Scripts.Patterns.Factory
{
    public interface IFactory<T>
    {
        T Create();
    }
}