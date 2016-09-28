using SqlBuilder.Interfaces;

namespace SqlBuilder.Interfaces
{
    public interface IDeleteBuilder : IWhereBuilder<IDeleteBuilder>
    {
        IDeleteBuilder Table(string table, string schema = "");

        BuildResult Build();
    }
}
