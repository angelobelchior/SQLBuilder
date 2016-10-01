using SqlBuilder.Interfaces;
using SqlBuilder.Util;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace SqlBuilder
{
    public class InsertBuilder : IInsertBuilder
    {
        private string _schema;
        private string _table;
        private Dictionary<string, SqlParameter> _values = new Dictionary<string, SqlParameter>();

        public IInsertBuilder Into(string table, string schema = "")
        {
            Throw.IfIsNullOrEmpty(table, nameof(table));

            this._table = table;
            this._schema = schema;
            if (!string.IsNullOrWhiteSpace(schema))
                this._schema += ".";

            return this;
        }

        public IInsertBuilder Value<T>(string column, T value)
        {
            var parameter = SqlDataExtentions.SqlParameterExtention.GetSqlParameter(column, value);
            this._values.Add(column, parameter);
            return this;
        }

        public BuildResult Build()
        {
            var sb = new StringBuilder();

            var columns = "";
            var values = "";
            foreach (var colum in this._values.Keys)
            {
                columns += $"[{colum}],";
                values +=  $"@{colum} ,";
            }

            columns = columns.RemoveLastChars();
            values = values.RemoveLastChars();

            sb.AppendLine($"{Constants.INSERT} {this._schema}{this._table}");
            sb.Append($"({columns}) ");
            sb.AppendLine($"{Constants.INSERT_VALUES}");
            sb.AppendLine($"({values})");

            var buildResult = new BuildResult(sb.ToString(), this._values.Values);
            return buildResult;
        }
    }
}