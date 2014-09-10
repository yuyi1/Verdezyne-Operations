using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security;
using System.Threading.Tasks;
using System.Web;
using Operations.Contracts;
using Operations.Models;

namespace Operations.RepositoryHelpers
{
    public class PilotPlantStoredProcedure : IPilotPlantRepositoryHelper
    {
        private PilotPlantEntities db;

        public PilotPlantStoredProcedure(PilotPlantEntities entities)
        {
            db = entities;
        }

        public void ExecuteNonQueryProcedure(string procedureName, System.Collections.Specialized.NameValueCollection parametersCollection)
        {
            try
            {
                string password = "wolfstein";
                var pwdarr = password.ToCharArray();
                SecureString securePwd = new SecureString();
                foreach (char c in pwdarr)
                {
                    securePwd.AppendChar(c);
                }
                securePwd.MakeReadOnly();

                using (
                    SqlConnection conn = new SqlConnection(this.db.Database.Connection.ConnectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = procedureName;
                    cmd.CommandType = CommandType.StoredProcedure;
                    foreach (var key in parametersCollection.AllKeys)
                    {
                        cmd.Parameters.AddWithValue(key, parametersCollection[key]);
                    }
                    var result = cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        //public async Task<int> ExecuteNonQueryProcedureAsync(string procedureName, NameValueCollection parametersCollection)
        //{
        //    try
        //    {
        //        using (SqlConnection conn = new SqlConnection(this.db.Database.Connection.ConnectionString))
        //        {
        //            conn.Open();
        //            SqlCommand cmd = new SqlCommand();
        //            cmd.Connection = conn;
        //            cmd.CommandText = procedureName;
        //            cmd.CommandType = CommandType.StoredProcedure;
        //            foreach (var key in parametersCollection.AllKeys)
        //            {
        //                cmd.Parameters.AddWithValue(key, parametersCollection[key]);
        //            }
        //            await cmd.ExecuteNonQuery();
        //            return 

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }            
        //}
    }
}