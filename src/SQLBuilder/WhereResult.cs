using System.Collections.Generic;
using System.Data.SqlClient;

namespace SQLBuilder
{
    internal class WhereResult
    {
        public string SQLCommand { get; set; }

        private List<SqlParameter> _parameters = new List<SqlParameter>();
        public IEnumerable<SqlParameter> Parameters => this._parameters;

        public void AddParameter(SqlParameter parameter) => this._parameters.Add(parameter);
        public void AddParameters(IEnumerable<SqlParameter> parameters) => this._parameters.AddRange(parameters);
    }
}