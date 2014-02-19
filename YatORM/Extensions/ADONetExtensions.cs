using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace YatORM.Extensions
{
    public static class ADONetExtensions
    {
        public static SqlCommand BuildCommand(
            this SqlConnection conn, 
            string commandText, 
            IEnumerable<SqlParameter> parameters = null, 
            CommandType commandType = CommandType.StoredProcedure)
        {
            var cmd = new SqlCommand(commandText, conn) { CommandType = commandType };

            if (parameters != null)
            {
                parameters.ForEach(p => cmd.Parameters.Add(p));
            }

            return cmd;
        }
    }
}