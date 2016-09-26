using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System;

namespace SQLBuilder
{
    internal class WhereBuilder
    {
        private readonly List<OperationBuilder> _filters;
        public WhereBuilder(List<OperationBuilder> filters)
        {
            this._filters = filters;
        }
        public BuildResult Build()
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
    }
}