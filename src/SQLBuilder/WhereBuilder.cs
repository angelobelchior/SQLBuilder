using System;
using System.Collections.Generic;
using SQLBuilder.Interfaces;
using System.Data.SqlClient;
using System.Text;

namespace SQLBuilder
{
    public class WhereBuilder<T> : IWhereBuilder<T>
    {
        private string _schema;
        private string _table;

        private string _condition;
        private string _column;
        private string _operator;
        private List<object> _values = new List<object>();
        private Func<bool> _function;

        private List<OperationBuilder> _filters = new List<OperationBuilder>();

        protected T Instance { get; set; }
        public T Where(string column)
        {
            this._column = column;
            return this.Instance;
        }

        internal BuildResult BuildWhere()
        {
            var SPACES = new Dictionary<string, string>
            {
                [Constants.CONDITION_AND] = "  ",
                [Constants.CONDITION_OR] = "   "
            };

            var parameters = new List<SqlParameter>();

            var sb = new StringBuilder();
            if (this._filters.Count > 0)
                sb.Append(Constants.WHERE);

            foreach (var filter in this._filters)
            {
                var spaces = string.Empty;
                if (!string.IsNullOrWhiteSpace(filter.Condition))
                    spaces = SPACES[filter.Condition];

                var paramters = filter.Build(spaces);
                sb.AppendLine(paramters.SQLCommand);
                parameters.AddRange(paramters.Parameters);
            }

            var sqlcommand = sb.ToString();
            sqlcommand.RemoveLastChars("\r\n".Length);

            return new BuildResult(sqlcommand, parameters);
        }

        protected void SetTableSchema(string table, string schema = "")
        {
            Throw.IfIsNullOrWhiteSpace(table, nameof(table));

            this._table = table;
            this._schema = schema;
            if (!string.IsNullOrWhiteSpace(schema))
                this._schema += ".";
        }

        protected string GetTableSchema() => $"{this._schema}{this._table}";

        public T And => this.AndOr(Constants.CONDITION_AND);
        public T Or => this.AndOr(Constants.CONDITION_OR);

        public T Diff(object value) => this.Operators(Constants.OPERATION_DIFF, value);
        public T Eq(object value) => this.Operators(Constants.OPERATION_EQ, value);
        public T Like(object value) => this.Operators(Constants.OPERATION_LIKE, value);
        public T Gt(object value) => this.Operators(Constants.OPERATION_GT, value);
        public T Lt(object value) => this.Operators(Constants.OPERATION_LT, value);
        public T GtEq(object value) => this.Operators(Constants.OPERATION_GT_EQ, value);
        public T LtEq(object value) => this.Operators(Constants.OPERATION_LT_EQ, value);
        public T Between(object value, object anotherValue) => this.Operators(Constants.OPERATION_BETWEEN, value, anotherValue);
        public T In(params object[] values) => this.Operators(Constants.OPERATION_IN, values);
        public T NotIn(params object[] values) => this.Operators(Constants.OPERATION_NOT_IN, values);

        public T If(bool condition) => this.If(() => condition);
        public T If<N>(Nullable<N> nullable) where N : struct => this.If(nullable.HasValue);
        public T If(Func<bool> function)
        {
            if (string.IsNullOrWhiteSpace(this._column)) throw new InvalidOperationException("Empty column");
            if (string.IsNullOrWhiteSpace(this._operator)) throw new InvalidOperationException("Empty operator");

            this._function = function;
            return this.Instance;
        }

        private T Operators(string @operator, params object[] values)
        {
            if (string.IsNullOrWhiteSpace(this._column)) throw new InvalidOperationException("Empty column");

            this._operator = @operator;
            foreach (var value in values)
                this._values.Add(value);

            return this.Instance;
        }

        private T AndOr(string andOr)
        {
            if (string.IsNullOrWhiteSpace(this._column)) throw new InvalidOperationException("Empty column");

            this.Restart();
            this._condition = andOr;
            return this.Instance;
        }

        private void Restart()
        {
            if (!string.IsNullOrWhiteSpace(this._column))
                if (this._function == null || (this._function != null && this._function()))
                    this._filters.Add(new OperationBuilder
                    {
                        Column = this._column,
                        Condition = this._condition,
                        Operation = this._operator,
                        Values = this._values,
                    });

            this._condition = string.Empty;
            this._column = string.Empty;
            this._operator = string.Empty;
            this._values = new List<object>(); ;
            this._function = null;
        }
    }
}