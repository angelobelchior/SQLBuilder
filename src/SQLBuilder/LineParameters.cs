using System.Collections.Generic;
using System.Data.SqlClient;

namespace SQLBuilder
{
    internal class LineParameters
    {
        public string Line { get; set; }
        public List<SqlParameter> Parameters { get; set; }

        public LineParameters()
        {
            this.Parameters = new List<SqlParameter>();
        }
    }
}