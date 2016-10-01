using System.Data;
using System.Data.SqlClient;

namespace SqlBuilder.SqlDataExtentions
{
    public static class SqlParameterExtention
    {
        public static SqlParameter GetSqlParameter(string parameterName, object value)
        {
            var parameter = new SqlParameter();
            parameter.ParameterName = parameterName;
            parameter.Value = value ?? System.DBNull.Value;
            parameter.DbType = GetSqlDbType(value);

            return parameter;
        }

        private static DbType GetSqlDbType(object value)
        {
            return DbType.Object;
        }   
    }
}