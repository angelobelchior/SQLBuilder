namespace SqlBuilder.Interfaces
{
    public interface IConditions<T>
    {
        T Between(object value, object anotherValue);
        T Diff(object value);
        T Eq(object value);
        T Gt(object value);
        T GtEq(object value);
        T Like(object value);
        T Lt(object value);
        T LtEq(object value);
        T Where(string column);
        T In(params object[] values);
        T NotIn(params object[] values);
    }
}
