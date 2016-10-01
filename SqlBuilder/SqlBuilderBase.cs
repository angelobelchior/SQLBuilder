using System;
using System.Collections.Generic;
using SqlBuilder.Interfaces;
using System.Data.SqlClient;
using System.Text;
using SqlBuilder.Util;

namespace SqlBuilder
{
    public abstract class SqlBuilderBase<T> : IWhereBuilder<T>
    {
        private string _schema;
        private string _table;

        private string _condition;
        private string _column;
        private string _operation;
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
                [Constants.WHERE_CONDITION_AND] = "  ",
                [Constants.WHERE_CONDITION_OR] = "   "
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
            Throw.IfIsNullOrEmpty(table, nameof(table));

            this._table = table;
            this._schema = schema;
            if (!string.IsNullOrWhiteSpace(schema))
                this._schema += ".";
        }

        protected string GetTableSchema() => $"{this._schema}{this._table}";

        public T And => this.AndOr(Constants.WHERE_CONDITION_AND);
        public T Or => this.AndOr(Constants.WHERE_CONDITION_OR);

        public T Diff<V>(V value) => this.Operations(Constants.WHERE_OPERATION_DIFF, value);
        public T Eq<V>(V value) => this.Operations(Constants.WHERE_OPERATION_EQ, value);
        public T Like<V>(V value) => this.Operations(Constants.WHERE_OPERATION_LIKE, value);
        public T Gt<V>(V value) => this.Operations(Constants.WHERE_OPERATION_GT, value);
        public T Lt<V>(V value) => this.Operations(Constants.WHERE_OPERATION_LT, value);
        public T GtEq<V>(V value) => this.Operations(Constants.WHERE_OPERATION_GT_EQ, value);
        public T LtEq<V>(V value) => this.Operations(Constants.WHERE_OPERATION_LT_EQ, value);
        public T Between<V>(V value, V anotherValue) => this.Operations(Constants.WHERE_OPERATION_BETWEEN, value, anotherValue);
        public T In<V>(params V[] values) => this.Operations(Constants.WHERE_OPERATION_IN, values);
        public T NotIn<V>(params V[] values) => this.Operations(Constants.WHERE_OPERATION_NOT_IN, values);

        public T If(bool condition) => this.If(() => condition);
        public T If<N>(Nullable<N> nullable) where N : struct => this.If(nullable.HasValue);
        public T If(Func<bool> function)
        {
            if (string.IsNullOrWhiteSpace(this._column)) throw new InvalidOperationException("Empty column");
            if (string.IsNullOrWhiteSpace(this._operation)) throw new InvalidOperationException("Empty operation");

            this._function = function;
            return this.Instance;
        }

        private T Operations<V>(string operation, params V[] values)
        {
            if (string.IsNullOrWhiteSpace(this._column)) throw new InvalidOperationException("Empty column");

            this._operation = operation;
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
                        Operation = this._operation,
                        Values = this._values,
                    });

            this._condition = string.Empty;
            this._column = string.Empty;
            this._operation = string.Empty;
            this._values = new List<object>(); ;
            this._function = null;
        }
    }
}