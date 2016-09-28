using System;

namespace SqlBuilder.Interfaces
{
    public interface IIf<T>
    {
        T If(bool condition);
        T If<N>(Nullable<N> nullable) where N : struct;
        T If(Func<bool> function);
    }
}
