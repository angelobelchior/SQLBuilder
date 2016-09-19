using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;

namespace SQLBuilder
{
    public class SQLBuilder<T> : IWhereBuilder<T>
    {
        private const string WHERE = "WHERE";

        private const string CONDITION_AND = "AND";
        private const string CONDITION_OR = "OR";

        private const string OPERATION_BETWEEN = "BETWEEN";
        private const string OPERATION_BETWEEN_VALUE_A = "_BETWEEN_A";
        private const string OPERATION_BETWEEN_VALUE_B = "_BETWEEN_B";

        private const string OPERATION_DIFF = "<>";
        private const string OPERATION_EQ = "=";
        private const string OPERATION_LIKE = "LIKE";
        private const string OPERATION_GT = ">";
        private const string OPERATION_LT = "<";
        private const string OPERATION_GT_EQ = ">=";
        private const string OPERATION_LT_EQ = "<=";

        private const string OPERATION_IN = "IN";
        private const string OPERATION_NOT_IN = "NOT IN";

        private string _condition;
        private string _column;
        private string _operator;
        private List<object> _values = new List<object>();
        private Func<bool> _function;

        private List<WhereItemFilter> _filters = new List<WhereItemFilter>();

        protected T Instance { get; set; }
        public T Where(string column)
        {
            this._column = column;
            return this.Instance;
        }

        internal WhereResult GetWhereCommand()
        {
            var SPACES = new Dictionary<string, string>
            {
                [CONDITION_AND] = "  ",
                [CONDITION_OR] = "   "
            };

            this.RestartWhere();

            var whereResult = new WhereResult();

            var sb = new StringBuilder();
            if (this._filters.Count > 0)
                sb.Append(WHERE);

            foreach (var filter in this._filters)
            {
                var spaces = string.Empty;
                if (!string.IsNullOrWhiteSpace(filter.Condition))
                    spaces = SPACES[filter.Condition];

                LineParameters lineParameter;
                if (filter.Operation.Equals(OPERATION_BETWEEN))
                    lineParameter = this.GetLineBetweenOperator(spaces, filter);
                else if (filter.Operation.Equals(OPERATION_IN) || filter.Operation.Equals(OPERATION_NOT_IN))
                    lineParameter = this.GetLineInNotInOperator(spaces, filter);
                else
                    lineParameter = this.GetLineCommonOperators(spaces, filter);

                sb.AppendLine(lineParameter.Line);
                whereResult.AddParameters(lineParameter.Parameters);
            }

            var command = sb.ToString();
            command.RemoveLastChars("\r\n".Length);

            whereResult.SQLCommand = command;
            return whereResult;
        }

        private LineParameters GetLineCommonOperators(string spaces, WhereItemFilter filter)
        {
            var lineParameters = new LineParameters();
            lineParameters.Line = $"{spaces}{filter.Condition} [{filter.Column}] {filter.Operation} @{filter.Column}";

            var parameter = this.GetSqlParameter(filter.Column, filter.Values[0]);
            lineParameters.Parameters.Add(parameter);

            return lineParameters;
        }

        private LineParameters GetLineBetweenOperator(string spaces, WhereItemFilter filter)
        {
            var lineParameters = new LineParameters();

            var parameterNameA = $"{filter.Column}{OPERATION_BETWEEN_VALUE_A}";
            var parameterNameB = $"{filter.Column}{OPERATION_BETWEEN_VALUE_B}";

            var parameterA = this.GetSqlParameter(parameterNameA, filter.Values[0]);
            var parameterB = this.GetSqlParameter(parameterNameB, filter.Values[1]);

            lineParameters.Parameters.Add(parameterA);
            lineParameters.Parameters.Add(parameterB);

            lineParameters.Line = $"{spaces}{filter.Condition} [{filter.Column}] {filter.Operation} @{parameterNameA} {CONDITION_AND} @{parameterNameB}";
            return lineParameters;
        }

        private LineParameters GetLineInNotInOperator(string spaces, WhereItemFilter filter)
        {
            var lineParameters = new LineParameters();

            var PART = new Dictionary<string, string>
            {
                [OPERATION_IN] = "IN",
                [OPERATION_NOT_IN] = "NOT_IN"
            };

            var list = new List<string>();
            for (int i = 0; i < filter.Values.Count; i++)
            {
                var parameterName = $"{ filter.Column }_{ PART[filter.Operation]}_{i}";
                list.Add($"@{parameterName}");

                var parameter = this.GetSqlParameter(parameterName, filter.Values[i]);
                lineParameters.Parameters.Add(parameter);
            }

            var parameters = string.Join(", ", list).Trim();
            lineParameters.Line = $"{spaces}{filter.Condition} [{filter.Column}] {filter.Operation} ({parameters})";
            return lineParameters;
        }

        private SqlParameter GetSqlParameter(string parameterName, object value)
        {
            var parameter = new SqlParameter(parameterName, value);
            //TODO: Configurar o parâmetro
            return parameter;
        }

        public T And => this.AndOr(CONDITION_AND);
        public T Or => this.AndOr(CONDITION_OR);

        public T Diff(object value) => this.Operators(OPERATION_DIFF, value);
        public T Eq(object value) => this.Operators(OPERATION_EQ, value);
        public T Like(object value) => this.Operators(OPERATION_LIKE, value);
        public T Gt(object value) => this.Operators(OPERATION_GT, value);
        public T Lt(object value) => this.Operators(OPERATION_LT, value);
        public T GtEq(object value) => this.Operators(OPERATION_GT_EQ, value);
        public T LtEq(object value) => this.Operators(OPERATION_LT_EQ, value);
        public T Between(object value, object anotherValue) => this.Operators(OPERATION_BETWEEN, value, anotherValue);
        public T In(params object[] values) => this.Operators(OPERATION_IN, values);
        public T NotIn(params object[] values) => this.Operators(OPERATION_NOT_IN, values);

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

            this.RestartWhere();
            this._condition = andOr;
            return this.Instance;
        }

        private void RestartWhere()
        {
            if (!string.IsNullOrWhiteSpace(this._column))
                if (this._function == null || (this._function != null && this._function()))
                    this._filters.Add(new WhereItemFilter
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