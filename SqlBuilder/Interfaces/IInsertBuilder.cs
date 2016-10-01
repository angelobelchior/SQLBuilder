namespace SqlBuilder.Interfaces
{
    public interface IInsertBuilder
    {
        IInsertBuilder Into(string table, string schema = "");
        IInsertBuilder Value<T>(string column, T value);
        BuildResult Build();
    }
}
