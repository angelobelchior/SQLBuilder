namespace SQLBuilder.Interfaces
{
    public interface ISelectBuilder : IWhereBuilder<ISelectBuilder>
    {
        ISelectBuilder Column(string column);
        ISelectBuilder Distinct();
        ISelectBuilder From(string table, string schema = "");
        ISelectBuilder Top(int top);

        BuildResult Build();
    }
}
