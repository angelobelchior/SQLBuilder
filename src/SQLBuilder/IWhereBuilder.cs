using System;

namespace SQLBuilder
{
    public interface IWhereBuilder<T>
    {
        T And { get; }
        T Or { get; }

        T Between(object value, object anotherValue);
        T Diff(object value);
        T Eq(object value);
        T Gt(object value);
        T GtEq(object value);
        T If(Func<bool> function);
        T If(bool condition);
        T If<N>(N? nullable) where N : struct;
        T Like(object value);
        T Lt(object value);
        T LtEq(object value);
        T Where(string column);
        T In(params object[] values);
        T NotIn(params object[] values);
    }
}