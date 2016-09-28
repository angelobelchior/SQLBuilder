using SqlBuilder.Interfaces;

namespace SqlBuilder
{
    public interface IDeleteBuilder : IWhereBuilder<IDeleteBuilder>
    {
        IDeleteBuilder Table(string table, string schema = "");

        BuildResult Build();
    }
}
