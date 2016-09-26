using SQLBuilder.SqlDataExtentions;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace SQLBuilder
{
    public class OperationBuilder
    {
        public string Condition { get; set; }
        public string Column { get; set; }
        public string Operation { get; set; }
        public List<object> Values { get; set; } = new List<object>();

        internal BuildResult Build(string spaces)
        {
            BuildResult buildResult;
            if (this.Operation.Equals(Constants.OPERATION_BETWEEN))
                buildResult = BuildBetweenOperation(spaces);
            else if (this.Operation.Equals(Constants.OPERATION_IN) || this.Operation.Equals(Constants.OPERATION_NOT_IN))
                buildResult = BuildInNotInOperation(spaces);
            else
                buildResult = BuildCommonOperation(spaces);

            return buildResult;
        }

        private BuildResult BuildCommonOperation(string spaces)
        {
            var sqlcommand = $"{spaces}{this.Condition} [{this.Column}] {this.Operation} @{this.Column}";
            var parameter = SqlParameterExtention.GetSqlParameter(this.Column, this.Values[0]);

            var buildResult = new BuildResult(sqlcommand, new List<SqlParameter> { parameter });
            return buildResult;
        }

        private BuildResult BuildBetweenOperation(string spaces)
        {
            var parameterNameA = $"{this.Column}{Constants.OPERATION_BETWEEN_VALUE_A}";
            var parameterNameB = $"{this.Column}{Constants.OPERATION_BETWEEN_VALUE_B}";

            var parameterA = SqlParameterExtention.GetSqlParameter(parameterNameA, this.Values[0]);
            var parameterB = SqlParameterExtention.GetSqlParameter(parameterNameB, this.Values[1]);

            var sqlcommand = $"{spaces}{this.Condition} [{this.Column}] {this.Operation} @{parameterNameA} {Constants.CONDITION_AND} @{parameterNameB}";

            var buildResult = new BuildResult(sqlcommand, new List<SqlParameter> { parameterA, parameterB });
            return buildResult;
        }

        private BuildResult BuildInNotInOperation(string spaces)
        {
            var PART = new Dictionary<string, string>
            {
                [Constants.OPERATION_IN] = "IN",
                [Constants.OPERATION_NOT_IN] = "NOT_IN"
            };

            var parameters = new List<SqlParameter>();

            var list = new List<string>();
            for (int i = 0; i < this.Values.Count; i++)
            {
                var parameterName = $"{ this.Column }_{ PART[this.Operation]}_{i}";
                list.Add($"@{parameterName}");

                var parameter = SqlParameterExtention.GetSqlParameter(parameterName, this.Values[i]);
                parameters.Add(parameter);
            }

            var text = string.Join(", ", list).Trim();
            var sqlcommand = $"{spaces}{this.Condition} [{this.Column}] {this.Operation} ({text})";

            var buildResult = new BuildResult(sqlcommand, parameters);
            return buildResult;
        }
    }
}
