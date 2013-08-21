using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace YatORM.Extensions
{
    public static class ADONetExtensions
    {
        public static SqlCommand BuildCommand(this SqlConnection conn, string queryName, IEnumerable<SqlParameter> parameters = null)
        {
            var cmd = new SqlCommand("dbo." + queryName, conn) { CommandType = CommandType.StoredProcedure };

            if (parameters != null)
            {
                parameters.ForEach(p => cmd.Parameters.Add(p));
            }
            return cmd;
        }
    }
}
