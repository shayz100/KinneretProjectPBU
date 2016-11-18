using Amsalem.Types.Misc.Cache;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amsalem.Types.Misc.DAL
{
    public class DataBaseAccess
    {
        public Exception LastError { get; set; }

        public static DataTable openConnectionGetResults(string connect, string select)
        {
            return openConnectionGetResultsP(connect, select);
        }

        public static DataTable openConnectionGetResultsP(string connect, string select, params DbParameter[] parameters)
        {
            DataBaseAccess external = new DataBaseAccess();
            return external.OpenConnectionGetResults(connect, select, parameters);
        }

        /// <summary>
        /// Static method to access all ADO.NET databases using the factory pattern 
        /// and a connection string name from the Web.Config file
        /// </summary>
        /// <param name="connect"></param>
        /// <param name="select">The SQL Query to execute</param>
        /// <returns></returns>
        public virtual DataTable OpenConnectionGetResults(string connect, string select, params DbParameter[] parameters)
        {
            var settings = ConfigurationManager.ConnectionStrings[connect];

            if (settings == null)
                settings = new ConnectionStringSettings("Ams", connect, "System.Data.SqlClient");

            DbProviderFactory factory = DbProviderFactories.GetFactory(settings.ProviderName);
            DataTable dt = new DataTable("Results");
            using (DbConnection conn = factory.CreateConnection())
            {
                conn.ConnectionString = settings.ConnectionString;
                conn.Open();
                DbCommand cmd = conn.CreateCommand();
                cmd.CommandText = select;
                DbDataAdapter adapter = factory.CreateDataAdapter();
                adapter.SelectCommand = cmd;
                cmd.CommandTimeout = 90000;
                foreach (var param in parameters)
                {
                    cmd.Parameters.Add(param);
                }
                try
                {
                    adapter.Fill(dt);
                    return dt;
                } //connection terminated
                catch (System.Exception)
                {
                    throw;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                    cmd.Dispose();
                    adapter.Dispose();
                }
            }
        }

        /// <summary>
        /// Execute ADO.NET sql query without results (insert, update, or stored procedure without results)
        /// </summary>
        /// <param name="connect"></param>
        /// <param name="select"></param>
        /// <returns></returns>
        public virtual int ExecuteQuery(string connect, string select, params DbParameter[] parmas)
        {
            try
            {
                var settings = ConfigurationManager.ConnectionStrings[connect];
                if (settings == null)
                    settings = new ConnectionStringSettings("Ams", connect, "System.Data.SqlClient");

                DbProviderFactory factory = DbProviderFactories.GetFactory(settings.ProviderName);
                using (DbConnection conn = factory.CreateConnection())
                {
                    conn.ConnectionString = settings.ConnectionString;
                    conn.Open();
                    DbCommand cmd = conn.CreateCommand();
                    cmd.CommandTimeout = 6000;
                    cmd.CommandText = select;
                    cmd.CommandType = CommandType.Text;
                    if (parmas != null)
                    {
                        foreach (var parma in parmas)
                        {
                            cmd.Parameters.Add(parma);
                        }
                    }
                    var result = cmd.ExecuteNonQuery();
                    conn.Close();
                    return result;
                } //connection terminated
            }
            catch (System.Exception e)
            {
                this.LastError = e;
                throw;
            }
        }

        public static string GetDataBaseName(string axCompany)
        {
            var connection = "";
            switch (axCompany)
            {
                case "AMSA":
                    connection = "AX_Israel";
                    break;
                case "USA":
                    connection = "AX_USA";
                    break;
                //case "AST":
                //    connection = "AX_Turkey";
                //    break;
                //case "IND":
                //    connection = "AX_India";
                //    break;
                //case "MEX":
                //    connection = "AX_MEX";
                //    break;
                //case "UK":
                //    connection = "AX_UK";
                //    break;
                //case "HKG":
                //    connection = "AX_HongKong";
                //    break;
                //case "DPC":
                //    connection = "DB_10_DpcNew";
                //    break;
                //case "AmsLogic_DB":
                //    connection = "AmsLogic_DBConnectionString";
                //    break;
                //case "AmsLogic_DTT":
                //    connection = "AmsLogic_DTT";
                //    break;
                default:
                    connection = "AX_Israel";
                    break;
            }
            return connection;
        }
        public static DataTable PerformQueryToCompany(bool AllCompany, string selectStatement, List<SqlParameter> SqlParameters, string dataAreaID)
        {
            DataTable TotalResults = null;
            if (AllCompany) //All Company
            {
                var CompanyList = GetAllCompanies();
                foreach (var Company in CompanyList)
                {
                    var ext = new DataBaseAccess();
                    var paramsToSend = new List<SqlParameter>();
                    foreach (var par in SqlParameters)
                    {
                        paramsToSend.Add(new SqlParameter(par.ParameterName, par.Value));
                    }
                    paramsToSend.Add(new SqlParameter("AxCompany", Company));
                    var results = ext.OpenConnectionGetResults(DataBaseAccess.GetDataBaseName(Company), selectStatement, paramsToSend.ToArray());
                    if (TotalResults != null && TotalResults.Rows.Count > 0)
                    {
                        foreach (DataRow row in results.Rows)
                        {
                            TotalResults.ImportRow(row);
                        }
                    }
                    else
                    {
                        TotalResults = results;
                    }
                }
            }

            else //Just my company
            {

                var ext = new DataBaseAccess();
                SqlParameters.Add(new SqlParameter("AxCompany", dataAreaID));
                var results = ext.OpenConnectionGetResults(DataBaseAccess.GetDataBaseName(dataAreaID), selectStatement, SqlParameters.ToArray());
                TotalResults = results;
            }
            return TotalResults;
        }
        public static List<string> GetAllCompanies()
        {
            var CompanyList = new List<string>();
            CompanyList.Add("AMSA");
            CompanyList.Add("USA");
            //CompanyList.Add("HKG");
            //CompanyList.Add("UK");
            //CompanyList.Add("MEX");
            //CompanyList.Add("AST");
            //CompanyList.Add("IND");
            return CompanyList;
        }

    }
}
