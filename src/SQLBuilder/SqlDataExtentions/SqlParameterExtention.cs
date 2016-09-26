using System.Data.SqlClient;

namespace SQLBuilder.SqlDataExtentions
{
    public static class SqlParameterExtention
    {
        public static SqlParameter GetSqlParameter(string parameterName, object value)
        {
            var parameter = new SqlParameter(parameterName, value);
            //TODO: Configurar o parâmetro
            return parameter;
        }
    }
}
