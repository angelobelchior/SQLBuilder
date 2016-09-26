using SQLBuilder.Interfaces;

namespace SQLBuilder
{
    public interface IDeleteBuilder : IWhereBuilder<IDeleteBuilder>
    {
        IDeleteBuilder Table(string table, string schema = "");

        BuildResult Build();
    }
}
