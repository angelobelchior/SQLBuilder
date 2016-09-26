using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLBuilder
{
    public class DeleteBuilder : SQLBuilder<IDeleteBuilder>, IDeleteBuilder
    {
        public DeleteBuilder()
        {
            this.Instance = this;
        }

        public IDeleteBuilder Table(string table, string schema = "")
        {
            this.SetTableSchema(table, schema);
            return this;
        }

        public BuildResult Build()
        {
            var sb = new StringBuilder();

            sb.AppendLine($"{Constants.DELETE} {this.GetTableSchema()}");

            var whereResult = this.BuildWhere();
            sb.Append(whereResult.SQLCommand);

            var buildResult = new BuildResult(sb.ToString(), whereResult.Parameters);
            return buildResult;
        }
    }
}
