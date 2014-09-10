using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Operations.Contracts
{
    public interface IRepositoryHelper<T> where T : class
    {
        void ExecuteNonQueryProcedure(string procedureName, NameValueCollection parametersCollection);
        //Task<SqlDataReader> ExecuteNonQueryProcedureAsync(string procedureName, NameValueCollection parametersCollection);
    }
}
