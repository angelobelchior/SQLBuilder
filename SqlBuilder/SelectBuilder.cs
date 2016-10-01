using SqlBuilder.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using SqlBuilder.Util;

namespace SqlBuilder
{
    public class SelectBuilder : SqlBuilderBase<ISelectBuilder>, ISelectBuilder
    {
        private string _distinct;
        private string _top;

        private List<string> _columns = new List<string>();

        public SelectBuilder()
        {
            this.Instance = this;
        }

        public ISelectBuilder Top(int top)
        {
            this._top = $"{Constants.SELECT_TOP} {top}";
            return this;
        }

        public ISelectBuilder Distinct()
        {
            this._distinct = Constants.SELECT_DISTINCT;
            return this;
        }

        public ISelectBuilder Column(string column)
        {
            this._columns.Add(column);
            return this;
        }

        public ISelectBuilder From(string table, string schema = "")
        {
            if (this._columns.Count == 0) throw new InvalidOperationException("Empty Columns");

            this.SetTableSchema(table, schema);
            
            return this;
        }

        public BuildResult Build()
        {
            var sb = new StringBuilder();
            sb.Append(Constants.SELECT);

            if (!string.IsNullOrWhiteSpace(this._distinct))
                sb.Append($" {this._distinct}");

            if (!string.IsNullOrWhiteSpace(this._top))
                sb.Append($" {this._top}");

            var columns = "\n";
            foreach (var column in this._columns)
                columns += Constants.SELECT_SELECT_SPACES + $"[{column}]" + Constants.SELECT_BREAK_LINE;

            sb.AppendLine(columns.RemoveLastChars(Constants.SELECT_BREAK_LINE.Length));
            sb.AppendLine($"{Constants.SELECT_FROM} {this.GetTableSchema()}");

            var whereResult = this.BuildWhere();
            sb.Append(whereResult.SQLCommand);

            var buildResult = new BuildResult(sb.ToString(), whereResult.Parameters);
            return buildResult;
        }
    }
}