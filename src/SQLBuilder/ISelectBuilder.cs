using System;
using System.Collections.Generic;
using System.Text;

namespace SQLBuilder
{
    public interface ISelectBuilder : IWhereBuilder<ISelectBuilder>
    {
        ISelectBuilder Column(string column);
        ISelectBuilder Distinct();
        ISelectBuilder From(string table, string schema = "");
        ISelectBuilder Top(int top);

        string GetSelectCommand();
    }

    public class SelectBuilder : SQLBuilder<ISelectBuilder>, ISelectBuilder
    {
        private const string SELECT = "SELECT";
        private const string FROM = "FROM";
        private const string DISTINCT = "DISTINCT";
        private const string TOP = "TOP";

        private string _schema;
        private string _table;
        private string _distinct;
        private string _top;

        private List<string> _columns = new List<string>();

        public SelectBuilder()
        {
            this.Instance = this;
        }

        public ISelectBuilder Top(int top)
        {
            this._top = $"{TOP} {top}";
            return this;
        }

        public ISelectBuilder Distinct()
        {
            this._distinct = DISTINCT;
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

            this._table = table;
            this._schema = schema;
            if (!string.IsNullOrWhiteSpace(schema))
                this._schema += ".";

            return this;
        }

        public string GetSelectCommand()
        {
            const string BREAK_LINE = ",\n";
            const string SELECT_SPACES = "       ";

            var sb = new StringBuilder();
            sb.Append(SELECT);

            if (!string.IsNullOrWhiteSpace(this._distinct))
                sb.Append($" {this._distinct}");

            if (!string.IsNullOrWhiteSpace(this._top))
                sb.Append($" {this._top}");

            var columns = "\n";
            foreach (var column in this._columns)
                columns += SELECT_SPACES + $"[{column}]" + BREAK_LINE;

            sb.AppendLine(columns.RemoveLastChars(BREAK_LINE.Length));
            sb.AppendLine($"{FROM} {this._schema}{this._table}");

            var whereResult = this.GetWhereCommand();
            sb.Append(whereResult.SQLCommand);

            return sb.ToString();
        }
    }
}