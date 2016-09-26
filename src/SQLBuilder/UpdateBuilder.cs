using SQLBuilder.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace SQLBuilder
{

    public class UpdateBuilder : WhereBuilder<IUpdateBuilder>, IUpdateBuilder
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

        public IUpdateBuilder Set(string column, object value)
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
                columns += $"{Constants.SET} [{colum.Key}] = @{colum.Key}" + Constants.BREAK_LINE;

            sb.AppendLine(columns.RemoveLastChars(Constants.BREAK_LINE.Length));

            var whereResult = this.BuildWhere();
            sb.Append(whereResult.SQLCommand);

            var buildResult = new BuildResult(sb.ToString(), whereResult.Parameters);
            return buildResult;
        }
    }
}
