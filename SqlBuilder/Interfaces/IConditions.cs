namespace SqlBuilder.Interfaces
{
    public interface IConditions<T>
    {
        T Between<V>(V value, V anotherValue);
        T Diff<V>(V value);
        T Eq<V>(V value);
        T Gt<V>(V value);
        T GtEq<V>(V value);
        T Like<V>(V value);
        T Lt<V>(V value);
        T LtEq<V>(V value);
        T Where(string column);
        T In<V>(params V[] values);
        T NotIn<V>(params V[] values);
    }
}
