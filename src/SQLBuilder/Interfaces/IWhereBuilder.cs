namespace SQLBuilder.Interfaces
{
    public interface IWhereBuilder<T> : IIf<T>, IOperators<T>, IConditions<T>
    {
    }
}