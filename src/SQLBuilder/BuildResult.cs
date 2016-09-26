using System.Collections.Generic;
using System.Data.SqlClient;

namespace SQLBuilder
{
    public class BuildResult
    {
        public string SQLCommand { get; private set; }
        public IEnumerable<SqlParameter> Parameters { get; private set; }

        public BuildResult(string sqlcommand, IEnumerable<SqlParameter> parameters)
        {
            this.SQLCommand = sqlcommand;
            this.Parameters = parameters;
        }
    }
}