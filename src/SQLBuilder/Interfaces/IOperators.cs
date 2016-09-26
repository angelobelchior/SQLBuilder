namespace SQLBuilder.Interfaces
{
    public interface IOperators<T>
    {
        T And { get; }
        T Or { get; }
    }
}
