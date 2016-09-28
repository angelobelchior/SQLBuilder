namespace SqlBuilder.Interfaces
{
    public interface IWhereBuilder<T> : IIf<T>, IOperators<T>, IConditions<T>
    {
    }
}