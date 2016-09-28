namespace SqlBuilder.Interfaces
{
    public interface IUpdateBuilder : IWhereBuilder<IUpdateBuilder>
    {
        IUpdateBuilder Table(string table, string schema = "");
        IUpdateBuilder Set(string column, object value);

        BuildResult Build();
    }
}
