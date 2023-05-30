using LarastruckingApp.Repository.Repository;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.Repository.EF
{
    public class ExecuteSQLStoredProceduce : LarastruckingDBEntities
    {
        public ExecuteSQLStoredProceduce()
        {
        }

        public List<T> ExecuteStoredProcedure<T>(string ProcName, List<SqlParameter> spParameters) where T : new()
        {
            List<T> result = new List<T>();

            //1. Open the Connection and Execute the Stored procedure.
            using (var con = this.Database.Connection)
            {
                con.Open();
                this.Database.CommandTimeout = 180;
                DbCommand cmd = this.Database.Connection.CreateCommand();

                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = ProcName;

                foreach (var parameter in spParameters)
                {
                    cmd.Parameters.Add(parameter);
                }
                if (cmd.Connection.State != System.Data.ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }

                using (var dr = cmd.ExecuteReader())
                {
                    result = SqlUtil.DataReaderToObjectList<T>(dr);
                }

            }
            return result;
        }
    }
}
