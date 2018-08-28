using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Dapper;

namespace HierarchyApp.DataAccess
{
    public class DataAccess
    {
        private readonly string _ConnString;


        public DataAccess(string cnString)
        {
            _ConnString = cnString;
        }


        public T execSP<T>(string sSPName, object oParameters) {
            using (var connection = new SqlConnection(_ConnString))
            {
                return connection.QueryFirstOrDefault<T>(sSPName, oParameters,
                    commandType: System.Data.CommandType.StoredProcedure);

            }
        }
    }
}
