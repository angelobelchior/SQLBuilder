namespace SqlBuilder.Interfaces
{
    public interface IUpdateBuilder : IWhereBuilder<IUpdateBuilder>
    {
        IUpdateBuilder Table(string table, string schema = "");
        IUpdateBuilder Set<T>(string column, T value);

        BuildResult Build();
    }
}
