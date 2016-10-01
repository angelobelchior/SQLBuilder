using SqlBuilder.Interfaces;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using SqlBuilder.Util;

namespace SqlBuilder
{

    public class UpdateBuilder : SqlBuilderBase<IUpdateBuilder>, IUpdateBuilder
    {
        private Dictionary<string, SqlParameter> _columns = new Dictionary<string, SqlParameter>();

        public UpdateBuilder()
        {
            this.Instance = this;
        }

        public IUpdateBuilder Table(string table, string schema = "")
        {
            this.SetTableSchema(table, schema);
            return this;
        }

        public IUpdateBuilder Set<T>(string column, T value)
        {
            var parameter = SqlDataExtentions.SqlParameterExtention.GetSqlParameter(column, value);
            this._columns.Add(column, parameter);
            return this;
        }

        public BuildResult Build()
        {
            var sb = new StringBuilder();

            sb.Append($"{Constants.UPDATE} {this.GetTableSchema()}");

            var columns = "\n";
            foreach (var colum in this._columns)
                columns += $"{Constants.UPDATE_SET} [{colum.Key}] = @{colum.Key}" + Constants.SELECT_BREAK_LINE;

            sb.AppendLine(columns.RemoveLastChars(Constants.SELECT_BREAK_LINE.Length));

            var whereResult = this.BuildWhere();
            sb.Append(whereResult.SQLCommand);

            var buildResult = new BuildResult(sb.ToString(), whereResult.Parameters);
            return buildResult;
        }
    }
}
